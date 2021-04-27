using AnnualLeave.Dtos;
using AnnualLeave.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AnnualLeave.Controllers.Api
{
    public class EmployeeController : ApiController
    {

        private ApplicationDbContext _context;

        public EmployeeController()
        {
            _context = new ApplicationDbContext();
        }

        //GET customers api/employees
        public IHttpActionResult GetEmployees()
        {
            var employeeDtos = _context.Employees.ToList().Select(Mapper.Map<Employee, EmployeeDto>);
            return Ok(employeeDtos);
        }

        //GET api/employees/1
        public IHttpActionResult GetEmployee(int id)
        {
            var employee = _context.Employees.SingleOrDefault(c => c.EmployeeId == id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<Employee,EmployeeDto>(employee));

        }

        //POST api/customers
        [HttpPost]
        public IHttpActionResult CreateEmployee(EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var employee = Mapper.Map<EmployeeDto,Employee>(employeeDto);

            _context.Employees.Add(employee);
            _context.SaveChanges();

            employeeDto.EmployeeId = employee.EmployeeId;

            return Created(new Uri(Request.RequestUri + "/" + employeeDto.EmployeeId), employeeDto);
        }

        //PUT api/customers/1
        [HttpPut]
        public IHttpActionResult UpdateEmployee(int id, EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var empInDb = _context.Employees.SingleOrDefault(e => e.EmployeeId == id);

            if (empInDb == null)
            {
                return NotFound();
            }

            Mapper.Map(employeeDto, empInDb);

            _context.SaveChanges();

            return Ok();

        }

        //DELETE api/employees/1
        [HttpDelete]
        public IHttpActionResult DeleteEmployee (int id)
        {
            var empInDb = _context.Employees.SingleOrDefault(e => e.EmployeeId == id);

            if (empInDb == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(empInDb);
            _context.SaveChanges();

            return Ok();
        }





    }
}
