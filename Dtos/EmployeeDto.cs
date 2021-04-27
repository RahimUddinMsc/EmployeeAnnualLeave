﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AnnualLeave.Dtos
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public int RoleID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public double MinutesAvailable { get; set; }
        [Required]
        public double MinutesUsed { get; set; }

    }
}