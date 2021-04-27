using AnnualLeave.Dtos;
using AnnualLeave.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnualLeave.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<Employee, EmployeeDto>();
            Mapper.CreateMap<EmployeeDto,Employee>();

            Mapper.CreateMap<Calendar, CalendarDto>();
            Mapper.CreateMap < CalendarDto, Calendar>();
        }


    }
}