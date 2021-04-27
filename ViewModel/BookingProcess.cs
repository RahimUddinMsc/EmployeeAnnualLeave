using AnnualLeave.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AnnualLeave.ViewModel
{
    public class BookingProcess
    {
        public IEnumerable<Employee> Employees { get; set; }

        public int EmployeeId { get; set; }
        public double MinutesRequested { get; set; }
        public Boolean CustomTime { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }


    }
}