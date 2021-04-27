using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using AnnualLeave.Models;
using AnnualLeave.ViewModel;
using Microsoft.AspNet.Identity;

namespace AnnualLeave.Controllers
{
    public class BookingController : Controller
    {

        private ApplicationDbContext _context;

        public BookingController()
        {
            _context = new ApplicationDbContext();            
        }


        public ActionResult Save(BookingProcess bookingProcess)
        {
            
            //find calendar date
            var startDate = bookingProcess.StartTime;            
            var daySelect = from dateChosen in _context.Calendars
                            where
                                dateChosen.Day == startDate.Day &&
                                dateChosen.Month == startDate.Month &&
                                dateChosen.Year == startDate.Year
                            select dateChosen;
            var dayObj = daySelect.FirstOrDefault();

            //get book obj data
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = _context.Users.FirstOrDefault(x => x.Id == currentUserId);            
            var approvalObj = _context.Approvals.Single(a => a.ApprovalId == 1);
            var calendarObj = _context.Calendars.Single(c => c.CalendarId == dayObj.CalendarId);

            var bookRecord = new Book
            {
                Calendar = calendarObj,
                CalendarId = calendarObj.CalendarId,
                Approval = approvalObj,
                ApprovalId = approvalObj.ApprovalId,
                CustomTimeSet = bookingProcess.CustomTime,
                MinutesAllocated = 60                
            };
            _context.Books.Add(bookRecord);

            //create the employee links and hook up both the employee and booking data
            var empLink = new EmployeeBookLink
            {
                User = currentUser,
                UserId = currentUserId,
                Book = bookRecord,
                BookId = bookRecord.BookId
            };
            _context.EmployeeBookLinks.Add(empLink);
            
            //finally save the data
            _context.SaveChanges();

            return Content("saved");
        }

        //setup the calender item
        public ActionResult SetupCalendarTable()
        {
            var startDate = DateTime.Parse("01/01/2020");
            var endDate = startDate.AddYears(1);

            while (startDate <= endDate)
            {
                startDate = startDate.AddDays(1);

                _context.Calendars.Add(new Calendar { 
                    Day = startDate.Day,
                    Month = startDate.Month,
                    Year = startDate.Year,
                    AvailableMinutes = 360                                                
                });
                
            }

            _context.SaveChanges();

            return Content("all Done");


        }



        // GET: Booking
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            return Content("hi" + id);
        }


        public ActionResult Option(int? num, string test)
        {
            if (!num.HasValue)
            {
                num = 10;
            }

            if (String.IsNullOrWhiteSpace(test))
            {
                test = "blank";
            }

            return Content(String.Format("id is {0} dBnd sort is {1}", num, test));

        }

        [Route("booking/custom/{name}/{year}")]
        public ActionResult Custom(string name, int year)
        {
            
            return Content(String.Format("id is {0} dBnd sort is {1}", name, year));

        }


    }
}