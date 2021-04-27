using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnualLeave.Dtos
{
    public class RequestDto
    {

        public RequestDto(List<StaffRequestUserDto> userList, List<StaffRequestDto> staffRequests, int approved, int pending, int declined)
        {

            UserList = userList;
            StaffRequests = staffRequests;
            Approved = approved;
            Pending = pending;
            Declined = declined;

        }

        public List<StaffRequestUserDto> UserList{ get; set; }
        public List<StaffRequestDto> StaffRequests { get; set; }

        public int Approved { get; set; }
        public int Pending { get; set; }
        public int Declined { get; set; }


    }

    public class StaffRequestDto
    {
        public int BookID { get; set; }
        
        public string UserID { get; set; }

        public string Date { get; set; }
        public double Minutes { get; set; }

        public string Reason { get; set; }

        public string Response { get; set; }


        public Boolean CustomTime { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

    }


   public class StaffRequestUserDto
   {

        public StaffRequestUserDto(string userID, string name)
        {
            UserID = userID;
            Name = name;
        }

        public string UserID { get; set; }
        public string Name { get; set; }



   }

}