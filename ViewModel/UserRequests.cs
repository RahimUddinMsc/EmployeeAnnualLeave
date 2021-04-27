using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnualLeave.ViewModel
{
    public class UserRequests
    {

        public int Approved { get; set; }
        public int Pending { get; set; }
        public int Declined { get; set; }

        public int BookID { get; set; }

        public List<UserRequestInfo> RequestList { get; set; }

    }

    public class UserRequestInfo
    {
        public int BookID { get; set; }
        public int CalendarId { get; set; }
        public int EmpLinkId { get; set; }

        public string Date { get; set; }
        public double Minutes { get; set; }

        public string Reason { get; set; }

        public string Response { get; set; }


        public Boolean CustomTime { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

    }

}