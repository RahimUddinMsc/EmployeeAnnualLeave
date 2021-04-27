using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnualLeave.Dtos
{
    public class UserInfoDto
    {
        public double MinutesAvailable  { get; set; }
        public double MinutesUsed { get; set; }
        public double FullDayAllocation { get; set; }


    }
}