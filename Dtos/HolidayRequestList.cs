using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnualLeave.Dtos
{
    public class HolidayRequestList
    {
        public IEnumerable<HolidayRequestDto> RequestList { get; set; }

    }
}