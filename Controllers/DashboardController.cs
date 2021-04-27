using AnnualLeave.Models;
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
    public class DashboardController : Controller
    {

        private ApplicationDbContext _context;

        public DashboardController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }


        // GET: Dashboard
        public ActionResult Index()
        {
            
            var userInfo = new DashboardInfo();
            userInfo.upcomingLeave = new List<string>();
            userInfo.NumRequest = 0;
            var userId = User.Identity.GetUserId();
            var curUser = _context.Users.FirstOrDefault(x => x.Id == userId);
            var holLinks = new List<EmployeeBookLink>();

            //add user info
            userInfo.Name = curUser.FirstName;
            userInfo.profileImg = curUser.ProfileImage;
            userInfo.MinutesAvailable = curUser.MinutesAvailable;
            userInfo.MinutesUsed = curUser.MinutesUsed;


            if (User.IsInRole(RoleName.CanManageStaff))
            {
                //get all the holiday records attached to the user
                holLinks = _context.EmployeeBookLinks.Include(e => e.Book)
                                                     .Include(e => e.User)
                                                     .Include(e => e.Book.Approval)
                                                     .Include(e => e.Book.Calendar).ToList();
            }
            else
            {
                //get all the holiday records attached to the user
                holLinks = _context.EmployeeBookLinks.Include(e => e.Book)
                                                     .Include(e => e.User)
                                                     .Include(e => e.Book.Approval)
                                                     .Include(e => e.Book.Calendar)
                                                     .Where(e => e.UserId == userId).ToList();
            }
                

            foreach (EmployeeBookLink obj in holLinks)
            {
                
                //loop through and increment for each request that is pending 
                if(obj.Book.ApprovalId == 3)
                {
                    userInfo.NumRequest += 1;
                }

                //for each request that has been approved add to list that is in the future
                var holDate = new DateTime(obj.Book.Calendar.Year, obj.Book.Calendar.Month, obj.Book.Calendar.Day);

                if (obj.Book.Approval.ApprovalId == 1 && holDate >= DateTime.Now && obj.UserId == userId)
                {              
                    userInfo.upcomingLeave.Add(holDate.ToString("dd/MM/yy"));
                }
            }
         
            if (User.IsInRole(RoleName.CanManageStaff))
            {
                //return View("AdminIndex",obj);
                return View("indexManage",userInfo);
            }
            else
            {

                //return View(obj);
                return View(userInfo);

            }

        }
    }
}