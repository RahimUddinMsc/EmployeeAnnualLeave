using AnnualLeave.Dtos;
using AnnualLeave.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using AnnualLeave.Migrations;
using System.Runtime.InteropServices;
using System.Data.Entity.Infrastructure;
using System.Web.WebPages.Instrumentation;
using System.Threading;
using System.Globalization;

namespace AnnualLeave.Controllers.Api
{
    public class CalendarApiController : ApiController
    {

        private ApplicationDbContext _context;

        private enum _HolStatus
        {
            Undefined,
            Approved = 1,
            Declined = 2,
            Pending = 3,
            Queue = 4,
            Insufficient_Holiday = 5
        }

        public CalendarApiController()
        {
            _context = new ApplicationDbContext();
        }


        public IHttpActionResult GetEmploy()
        {
            var employeeDtos = _context.Employees.ToList().Select(Mapper.Map<Employee, EmployeeDto>);
            return Ok(employeeDtos);
        }

        [Route("GetBar")]
        public IHttpActionResult GetBar()
        {
            var employeeDtos = _context.Employees.ToList().Select(Mapper.Map<Employee, EmployeeDto>);
            return Ok(employeeDtos);
        }

        [Route("GetCalendarData")]
        public IHttpActionResult GetCalendarData (int monthSelected, int yearSelected)
        {
            //var employeeDtos = _context.Employees.ToList().Select(Mapper.Map<Employee, EmployeeDto>);
            var calendarDtos = _context.Calendars.Where(r => r.Month == monthSelected && r.Year == yearSelected).ToList();
            return Ok(calendarDtos);
        }

        [Route("GetUserInfo")]
        public IHttpActionResult GetUserInfo()
        {
            var userId = User.Identity.GetUserId();
            ApplicationUser currentUser = _context.Users.FirstOrDefault(x => x.Id == userId);
            var userHolInfo = _context.UserHolidayInfos.FirstOrDefault(u => u.UserId == userId);
            
            var userInfoObj = new UserInfoDto
            {
                MinutesAvailable = currentUser.MinutesAvailable,
                MinutesUsed = currentUser.MinutesUsed,
                FullDayAllocation = userHolInfo.FullDay
            };
            return Ok(userInfoObj);
        }


        [Route("GetUserHolidaysForMonth")]
        public IHttpActionResult GetUserHolidaysForMonth(int monthSelected, int yearSelected)
        {

            var userId = User.Identity.GetUserId();

            //get all the holiday records attached to the user
            var holLinks = _context.EmployeeBookLinks.Include(e => e.Book)
                                                     .Include(e => e.User)
                                                     .Include(e => e.Book.Approval)
                                                     .Where(e => e.UserId == userId && e.Book.Calendar.Month == monthSelected && e.Book.Calendar.Year == yearSelected).ToList();                       
            var holList = new List<HolidayAllocationDto>();

            foreach (EmployeeBookLink obj in holLinks)
            {
                //get queue for pos if there is one avaialible by checking if record exist with the calendar id and user id
                var qPos = -1;
                var qRec = _context.Queue.Where(e => e.UserId == userId & e.CalendarId == obj.Book.CalendarId).SingleOrDefault();
                if(qRec != null)
                {
                    qPos = qRec.position;
                }

                var pendingObj = _context.Pendings.Where(p => p.BookId == obj.BookId).SingleOrDefault();
                if(pendingObj == null) { pendingObj = new Pending { }; }

                var customTime = _context.CustomTimes.Where(c => c.CalendarId == obj.Book.CalendarId).SingleOrDefault();
                if (customTime == null) { customTime = new CustomTime { }; }
               
                holList.Add(new HolidayAllocationDto()
                    {
                        EmpLinkId = obj.EmployeeBookLinkId,
                        BookId = obj.BookId,
                        CalendarId = obj.Book.CalendarId,
                        Approval = obj.Book.Approval,
                        CustomTimeSet = obj.Book.CustomTimeSet,
                        CustomTime = customTime,
                        NumQueue = qPos,                        
                        Pending = pendingObj,                        
                    }
                );
            }
           
            return Ok(holList);
       
        }


        [Route("RequestHoliday")]
        [HttpPost]
        public IHttpActionResult RequestHoliday(HolidayRequestList holidayRequestList)
        {

            var userId = User.Identity.GetUserId();
            ApplicationUser currentUser = _context.Users.FirstOrDefault(x => x.Id == userId);

            foreach (HolidayRequestDto holObj in holidayRequestList.RequestList)
            {

                
                var calendarObj = _context.Calendars.Single(c => c.CalendarId == holObj.CalendarId);

                //default to approved holiday initially then change according to conditions
                var approvalObj = _context.Approvals.Single(a => a.ApprovalId == (int)_HolStatus.Approved);

                //if minutes requested more then holiday available pull '5 - insufficient holiday'
                if (holObj.MinutesRequested > currentUser.MinutesAvailable)
                {
                    approvalObj = _context.Approvals.Single(a => a.ApprovalId == (int)_HolStatus.Insufficient_Holiday);
                }
                else
                {
                    //if calendar available mins goes negative then grab approval obj of '4 - queue'
                    if ((calendarObj.AvailableMinutes -= holObj.MinutesRequested) < 0)
                    {
                        approvalObj = _context.Approvals.Single(a => a.ApprovalId == (int)_HolStatus.Queue);
                    }
                   
                }                            
               
                var bookRecord = new Book
                {
                    Calendar = calendarObj,
                    CalendarId = calendarObj.CalendarId,
                    Approval = approvalObj,
                    ApprovalId = approvalObj.ApprovalId,
                    CustomTimeSet = holObj.CustomTime,
                    MinutesAllocated = holObj.MinutesRequested,
                    DateTimeRequested = DateTime.Now
                };
                _context.Books.Add(bookRecord);

                //if holiday is a custom time then add start and end dates
                if (holObj.CustomTime)
                {

                    var customTimeRec = new CustomTime
                    {
                        Calendar = calendarObj,
                        CalendarId = calendarObj.CalendarId,
                        StartTime = holObj.Start,
                        EndTime = holObj.End
                    };
                    _context.CustomTimes.Add(customTimeRec);
                }


                //create the employee links and hook up both the employee and booking data
                var empLink = new EmployeeBookLink
                {
                    User = currentUser,
                    UserId = currentUser.Id,
                    Book = bookRecord,
                    BookId = bookRecord.BookId
                };
                _context.EmployeeBookLinks.Add(empLink);           
                

                //add to a queue record if approval id is also set to queue
                if(approvalObj.ApprovalId == (int)_HolStatus.Queue)
                {
                    //run check to see if there allready queue for the day so can also increment position val if required 
                    var maxRec = _context.Queue.OrderByDescending(m => m.CalendarId == holObj.CalendarId).Where(m => m.CalendarId == holObj.CalendarId).FirstOrDefault();                    
                    var pos = 0;
                    if (maxRec == null)
                    {
                        pos += 1;
                    }
                    else
                    {
                        pos = (maxRec.position + 1);
                    }

                    var queueObj = new Queue
                    {
                        Calendar = calendarObj,
                        CalendarId = holObj.CalendarId,
                        User = currentUser,
                        UserId = userId,
                        position = pos                        
                    };
                    _context.Queue.Add(queueObj);

                }

                //if hol has been approved then decrement the users' holiday allocation              
                if (approvalObj.ApprovalId != (int)_HolStatus.Insufficient_Holiday) { 
                    currentUser.MinutesAvailable -= bookRecord.MinutesAllocated;
                    currentUser.MinutesUsed += bookRecord.MinutesAllocated;
                }                
            }

            _context.SaveChanges();

            //var employeeDtos = _context.Employees.ToList().Select(Mapper.Map<Employee, EmployeeDto>);            
            return Ok(holidayRequestList);
        }


        [Route("CancelHoliday")]
        [HttpDelete]
        public IHttpActionResult CancelHoliday(int calendarId, int bookId, int empLinkId)
        {

            var userId = User.Identity.GetUserId();
            var bookObj = _context.Books.Single(b => b.BookId == bookId);

            if(bookObj.ApprovalId == (int)_HolStatus.Insufficient_Holiday)
            {
                ExecuteHolidayCancellation(userId, calendarId, bookId, empLinkId, false);
            }
            else
            {
                ExecuteHolidayCancellation(userId, calendarId, bookId, empLinkId, true);
            }
            


            //var calendarObj = _context.Calendars.Single(c => c.CalendarId == calendarId);            
            //var bookObj = _context.Books.Single(b => b.BookId == bookId);
            //var empLinkObj = _context.EmployeeBookLinks.Single(e => e.EmployeeBookLinkId == empLinkId);
            ////delete associated queue/pending items if there is one  item if there is one
            //var queueObj = _context.Queue.SingleOrDefault(q => q.CalendarId == calendarObj.CalendarId && q.UserId == userId);
            //var pendingObj = _context.Pendings.SingleOrDefault(p => p.BookId == bookObj.BookId);

            ////update holiday minutes
            //calendarObj.AvailableMinutes += bookObj.MinutesAllocated; 

            //_context.EmployeeBookLinks.Remove(empLinkObj);
            //_context.Books.Remove(bookObj);
            //if (queueObj != null) { _context.Queue.Remove(queueObj); }
            //if (pendingObj != null) { _context.Pendings.Remove(pendingObj); }

            //_context.SaveChanges();

            //var employeeDtos = _context.Employees.ToList().Select(Mapper.Map<Employee, EmployeeDto>);            
            return Ok("done");
        }

        public void ExecuteHolidayCancellation(string userId, int calendarId, int bookId, int empLinkId, Boolean ammendAllocation)
        {

            ApplicationUser currentUser = _context.Users.FirstOrDefault(x => x.Id == userId);
            var calendarObj = _context.Calendars.Single(c => c.CalendarId == calendarId);            
            var bookObj = _context.Books.Single(b => b.BookId == bookId);
            var empLinkObj = _context.EmployeeBookLinks.Single(e => e.EmployeeBookLinkId == empLinkId);
            //delete associated queue/pending items if there is one  item if there is one
            var queueObj = _context.Queue.SingleOrDefault(q => q.CalendarId == calendarObj.CalendarId && q.UserId == userId);
            var pendingObj = _context.Pendings.SingleOrDefault(p => p.BookId == bookObj.BookId);
            var queuePos = -1; 

            //update holiday minutes
            if (ammendAllocation) { 
                calendarObj.AvailableMinutes += bookObj.MinutesAllocated;
                currentUser.MinutesUsed -= bookObj.MinutesAllocated;
                currentUser.MinutesAvailable += bookObj.MinutesAllocated;
            };

            _context.EmployeeBookLinks.Remove(empLinkObj);
            if (bookObj.CustomTimeSet)
            {
                var customObj = _context.CustomTimes.SingleOrDefault(c => c.CalendarId == bookObj.CalendarId);
                _context.CustomTimes.Remove(customObj);
            }

            _context.Books.Remove(bookObj);
            if (queueObj != null) {
                queuePos = queueObj.position;
                _context.Queue.Remove(queueObj); 
            }
            if (pendingObj != null) { _context.Pendings.Remove(pendingObj); }

            _context.SaveChanges();
            //adjust any queues on that day if there is any
            AdjustHolidayQueues(calendarId,queuePos);
        }


        public void AdjustHolidayQueues(int calendarId, int setPos)
        {
            var qMax = _context.Queue.OrderByDescending(m => m.CalendarId == calendarId).Where(m => m.CalendarId == calendarId).FirstOrDefault();                        
            var canBookHol = true;
            
            if(qMax != null && setPos != -1 )
            {             
                //grab the queue postion and decrement
                var pos = setPos+1;
               
                //do a while loop until t
                while (pos <= qMax.position)
                {
                    
                    var queueObj = _context.Queue.Where(q => q.CalendarId == calendarId && q.position == pos).SingleOrDefault();
                    var userObj = _context.EmployeeBookLinks.Include(e => e.Book).Include(e => e.User).Include(e => e.Book.Approval).Where(e => e.UserId == queueObj.UserId && e.Book.CalendarId == calendarId).SingleOrDefault();                    
                    var calendarObj = _context.Calendars.SingleOrDefault(c => c.CalendarId == queueObj.CalendarId);
                    var pendingObj = _context.Pendings.SingleOrDefault(p => p.BookId == userObj.Book.BookId);

                    //At the start of each loop assume can book holiday using global bar do a check for this until changed by condition
                    if (userObj.Book.MinutesAllocated > calendarObj.AvailableMinutes) { canBookHol = false; }


                    // if can book holiday this would mean queue record delete or any request records available would need to be del 
                    // will need to run recursively has if holiday booked then pos number reset
                    if (canBookHol)
                    {
                        if(userObj.User.MinutesAvailable > userObj.Book.MinutesAllocated)
                        {
                            userObj.Book.ApprovalId = (int)_HolStatus.Approved;
                            _context.Queue.Remove(queueObj);
                            if (pendingObj != null)
                            {
                                pendingObj.approved = (int)_HolStatus.Approved;
                                pendingObj.Response = "Holiday became available";
                            }
                            calendarObj.AvailableMinutes -= userObj.Book.MinutesAllocated;

                            userObj.User.MinutesAvailable -= userObj.Book.MinutesAllocated;
                            userObj.User.MinutesUsed += userObj.Book.MinutesAllocated;


                        }
                        else
                        {
                            userObj.Book.ApprovalId = (int)_HolStatus.Insufficient_Holiday;
                            _context.Queue.Remove(queueObj);
                            if (pendingObj != null)
                            {
                                pendingObj.approved = (int)_HolStatus.Insufficient_Holiday;
                                pendingObj.Response = "Holiday became available but you do not have enough holiday available";
                            }
                        }
                                                
                        _context.SaveChanges();
                        //exits while loop so recursion stops at correct time
                        AdjustHolidayQueues(calendarId,pos);
                        pos = qMax.position + 1;
                    }

                    else
                    {
                        //if unable to book to do this just decrement number in the queue 
                        queueObj.position = pos-1;
                        calendarObj.AvailableMinutes += userObj.Book.MinutesAllocated;
                        _context.SaveChanges();

                    }

                    pos += 1;


                }

            }

        }


        [Route("RequestHolidayOveride")]
        [HttpPost]
        public IHttpActionResult RequestHolidayOveride(int bookId, string reason)
        {

            var userId = User.Identity.GetUserId();
            ApplicationUser currentUser = _context.Users.FirstOrDefault(x => x.Id == userId);

            var bookObj = _context.Books.Single(b => b.BookId == bookId);

            //Make an actual pending record to attach to book record
            var pendingObj = new Pending
            {

                BookId = bookObj.BookId,
                Book = bookObj,
                Reason = reason,
                Response = "Awaiting Response",
                approved = 0
            };

            //id to show is pending with manager
            bookObj.ApprovalId = (int)_HolStatus.Pending;

            _context.Pendings.Add(pendingObj);
            _context.SaveChanges();

            //var employeeDtos = _context.Employees.ToList().Select(Mapper.Map<Employee, EmployeeDto>);            
            return Ok("done");
        }

        [Route("UpdateAllocationDays")]
        [HttpPost]
        public IHttpActionResult UpdateAllocationDays(IEnumerable<AllocationRequest> RequestList)
        {

            foreach (AllocationRequest req in RequestList)
            {
                var calDay = _context.Calendars.Single(e => e.CalendarId == req.CalendarId);
                calDay.AvailableMinutes = req.TimeSet;
            }

            _context.SaveChanges();
            
            return Ok("done");

        }


        [Route("StaffRequestVerdict")]
        [HttpPost]
        public IHttpActionResult StaffRequestVerdict(int bookId, bool approved, string reason)
        {

            //grab the employee and book data
            //grab the pending obj
            var holLinks = _context.EmployeeBookLinks.Include(e => e.Book)
                                                     .Include(e => e.User)
                                                     .Include(e => e.Book.Approval)
                                                     .Where(e => e.BookId == bookId).FirstOrDefault();
            var pending = _context.Pendings.FirstOrDefault(b => b.BookId == bookId);
            var approvalObj = new Approval();
            var queueObj = _context.Queue.SingleOrDefault(q => q.CalendarId == holLinks.Book.CalendarId && q.UserId == holLinks.User.Id);

            //get the approval item d
            if (approved)
            {
                //check to see if the user has the appropiate anmount of minutes available for the holiday if not set to inneficient
                if (holLinks.Book.MinutesAllocated > holLinks.User.MinutesAvailable)
                {
                    approvalObj = _context.Approvals.Single(a => a.ApprovalId == (int)_HolStatus.Approved);
                }
                else
                {
                    approvalObj = _context.Approvals.Single(a => a.ApprovalId == (int)_HolStatus.Approved);
                }

            } 
            else
            { 
                approvalObj = _context.Approvals.Single(a => a.ApprovalId == (int)_HolStatus.Declined);
            }
            
            //update the calendar minutes if has declined        
            if (approvalObj.ApprovalId == (int)_HolStatus.Declined)
            {                
                //holLinks.Book.Calendar.AvailableMinutes -= holLinks.Book.MinutesAllocated;
                holLinks.User.MinutesUsed -= holLinks.Book.MinutesAllocated;
            }

            //delete and update the queue
            if (queueObj != null)
            {
                var pos = queueObj.position;
                _context.Queue.Remove(queueObj);
                AdjustHolidayQueues(holLinks.Book.CalendarId, pos);
            }

            //update book obj
            //update the pending obj with decision and response
            holLinks.Book.Approval = approvalObj;
            holLinks.Book.ApprovalId = approvalObj.ApprovalId;
            pending.approved = approvalObj.ApprovalId;
            pending.Response = reason;

            //queue update still required

            _context.SaveChanges();
           
            //var employeeDtos = _context.Employees.ToList().Select(Mapper.Map<Employee, EmployeeDto>);            
            return Ok("done");
        }





    }
}
