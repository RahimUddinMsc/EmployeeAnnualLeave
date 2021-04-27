using AnnualLeave.Models;
using AnnualLeave.Dtos;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using AnnualLeave.ViewModel;

namespace AnnualLeave.Controllers
{
    
    public class RequestController : Controller
    {

        private ApplicationDbContext _context;

        public RequestController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Request
        [Authorize(Roles = RoleName.CanManageStaff)]
        public ActionResult IndexManage()
        {
            var userId = User.Identity.GetUserId();
            ApplicationUser managerUser = _context.Users.FirstOrDefault(x => x.Id == userId);          
            var managerAttached = new List<EmployeeBookLink>();
            var pendingReqs = _context.Pendings.Include(p => p.Book).ToList();                                                
                                               

            //Grab all the items which are in the pending table
            foreach (Pending obj in pendingReqs)
            {
                
                var holLink = _context.EmployeeBookLinks.Include(b => b.Book)
                                         .Include(u => u.User)
                                         .Include(a => a.Book.Approval)
                                         .Include(c => c.Book.Calendar)
                                         .Where(b => b.BookId == obj.BookId).ToList();               

                //exlude any pending items whos' team members who are not part of the managers' team
                foreach (EmployeeBookLink hol in holLink)
                {
                    var checkUser = _context.Teams.Include(u => u.ApplicationUser)
                                                 .FirstOrDefault(u => u.UserID == hol.UserId && u.ManagerID == managerUser.ManagerID);

                    if(checkUser != null)
                    {
                        managerAttached.Add(hol);
                    }

                }

            }

            var usersIds = new List<string>();
            var userList = new List<StaffRequestUserDto>();
            var staffReqs = new List<StaffRequestDto>();
            var approved = 0;
            var pending = 0;
            var declined = 0;
         
            //look at the following for some inspiration
            //GetUserHolidaysForMont
            foreach (EmployeeBookLink obj in managerAttached)
            {
            
                //only add user to the list if is unique will be used for sorting on client side razor pages
                if (!usersIds.Contains(obj.User.Id))
                {
                    usersIds.Add(obj.User.Id);
                    userList.Add(new StaffRequestUserDto(obj.User.Id, obj.User.FirstName + " " + obj.User.LastName));
                }


                //get totals for requests
                switch (obj.Book.Approval.ApprovalId)
                {
                    case 1:
                        approved += 1;
                        break;
                    case 2:
                        declined += 1;
                        break;
                    case 3:
                        pending += 1;
                        break;
                }

                if (obj.Book.Approval.ApprovalId != 3)
                {
                    continue;
                }

                //make an obj which will hold data for each individual request
                var holDate = new DateTime(obj.Book.Calendar.Year, obj.Book.Calendar.Month, obj.Book.Calendar.Day);
                var req = new StaffRequestDto()
                {
                    BookID = obj.BookId,
                    UserID = obj.User.Id,             
                    Date = holDate.ToString("dd/MM/yy"),
                    Minutes = obj.Book.MinutesAllocated,
                    CustomTime = obj.Book.CustomTimeSet,                    
                };

                //get the pending reasons for the request
                var comms = _context.Pendings.FirstOrDefault(p => p.BookId == obj.BookId);
                req.Reason = comms.Reason;
                req.Response = comms.Response;

                //if has custom time ensure that custom time is added 
                if (req.CustomTime)
                {
                    var customObj = _context.CustomTimes.SingleOrDefault(c => c.CalendarId == obj.Book.CalendarId);
                    req.StartTime = customObj.StartTime;
                    req.EndTime = customObj.EndTime;
                }

                staffReqs.Add(req);

            }

            //create model for the view page          
            return View("IndexManage",new RequestDto(userList,staffReqs,approved,pending,declined));
        }

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var userRequest = new UserRequests();
            userRequest.RequestList = new List<UserRequestInfo>();

            // get all the pending book items for the the user 
            var holLinks = _context.EmployeeBookLinks.Include(e => e.Book)
                                                     .Include(e => e.User)
                                                     .Include(e => e.Book.Approval)
                                                     .Include(e => e.Book.Calendar)
                                                     .Where(e => e.UserId == userId).ToList();

            //look at the following for some inspiration
            //GetUserHolidaysForMont
            foreach (EmployeeBookLink obj in holLinks)
            {
               
                //get totals for requests
                switch (obj.Book.Approval.ApprovalId)
                {
                    case 1:
                        userRequest.Approved += 1;
                        break;
                    case 2:
                        userRequest.Declined += 1;
                        break;
                    case 3:
                        userRequest.Pending += 1;
                        break;
                }

                //make an obj which will hold data for each individual request                
                var holDate = new DateTime(obj.Book.Calendar.Year, obj.Book.Calendar.Month, obj.Book.Calendar.Day);
                var req = new UserRequestInfo()
                {
                    BookID = obj.BookId,                    
                    CalendarId = obj.Book.CalendarId,
                    EmpLinkId = obj.EmployeeBookLinkId,
                    Date = holDate.ToString("dd/MM/yy"),
                    Minutes = obj.Book.MinutesAllocated,
                    CustomTime = obj.Book.CustomTimeSet,
                };
               
                //if has custom time ensure that custom time is added 
                if (req.CustomTime)
                {
                    var customObj = _context.CustomTimes.SingleOrDefault(c => c.CalendarId == obj.Book.CalendarId);
                    req.StartTime = customObj.StartTime;
                    req.EndTime = customObj.EndTime;
                }

                //get the pending reasons for the request
                var comms = _context.Pendings.FirstOrDefault(p => p.BookId == obj.BookId);
                if(comms != null)
                {                    
                    req.Reason = comms.Reason;
                    req.Response = comms.Response == "" ? "Awaiting Response" : comms.Response;
                    userRequest.RequestList.Add(req);
                }
                

            }

            //create model for the view page          
            return View(userRequest);
        }


    }
}