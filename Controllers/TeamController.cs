using AnnualLeave.Models;
using AnnualLeave.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;


namespace AnnualLeave.Controllers
{

    [Authorize(Roles = RoleName.CanManageStaff)]
    public class TeamController : Controller
    {

        private ApplicationDbContext _context;

        public TeamController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Team
        public ActionResult Index()
        {

            //will only ever hit this action if is a user so get the manager id of who is currently logged in
            var userId = User.Identity.GetUserId();
            ApplicationUser manageUser = _context.Users.FirstOrDefault(x => x.Id == userId);
            var managerID = manageUser.ManagerID;

            //setup view model obj
            var teamList = new List<TeamHandler>();

            //grab all associated users to the manager
            var attachedUsers = _context.Teams.Where(u => u.ManagerID == managerID).ToList();

            foreach(Team member in attachedUsers)
            {


                //Start formulating the user obj 
                var handler = new TeamHandler();                
                var holList = new List<TeamUserHolidayInfo>();
                var onHol = false;
                var user = _context.Users.FirstOrDefault(u => u.Id == member.UserID);

                //Get and Loop through all holidays that have been approved and attach them to the user model                
                var holLink = _context.EmployeeBookLinks.Include(b => b.Book)
                                                         .Include(u => u.User)                                          
                                                         .Include(c => c.Book.Calendar)
                                                         .Where(u => u.User.Id == member.UserID && u.Book.ApprovalId == 1).ToList();

                foreach(EmployeeBookLink hol in holLink)
                {
                    
                    var holObj = new TeamUserHolidayInfo();
                    var holDate = new DateTime(hol.Book.Calendar.Year, hol.Book.Calendar.Month, hol.Book.Calendar.Day);                    
                    holObj.Date = holDate.ToString("dd/MM/yy");
                    holObj.CustomTime = hol.Book.CustomTimeSet;

                    if (holObj.CustomTime)
                    {
                        var customObj = _context.CustomTimes.SingleOrDefault(c => c.CalendarId == hol.Book.CalendarId);
                        holObj.StartTime = customObj.StartTime;
                        holObj.EndTime = customObj.EndTime;
                    }

                    //check to see if the user is currently on holiday today for assignment later
                    if(holDate == new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                    {
                        onHol = true;
                    }

                    if(holDate >= DateTime.Today)
                    {
                        holList.Add(holObj);
                    }
                    

                }


                handler.OnHoliday = onHol;
                handler.Name = user.FirstName + " " + user.LastName;
                handler.MinutesAvailable = user.MinutesAvailable;
                handler.MinutesUsed = user.MinutesUsed;
                handler.profileImg = user.ProfileImage;

                //attach holidays to user and the handler to the parent team list
                handler.UserHolidayInfo = holList;
                teamList.Add(handler);
             
            }
         
            //sent the obj back to the 
            return View(teamList);
        }
    }
}