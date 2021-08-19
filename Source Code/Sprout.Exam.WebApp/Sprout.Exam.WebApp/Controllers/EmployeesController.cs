using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Common.Enums;
using Microsoft.Extensions.Configuration;
using Sprout.Exam.DataAccess;

namespace Sprout.Exam.WebApp.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        Employee_DA employee_DA = null;
        EmployeeDto employeeDto = null;
        List<EmployeeDto> lstEmployeeDto = null;
        string connectionString = String.Empty;
       
        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            
            //var result = await Task.FromResult(StaticEmployees.ResultList);
            employee_DA = new Employee_DA();
            try
            {
                lstEmployeeDto = new List<EmployeeDto>();
                lstEmployeeDto = employee_DA.GetList().ToList();
            }
            catch (Exception ex)
            {
                employeeDto = new EmployeeDto();
                employeeDto.MessageList = new List<string>();
                employeeDto.MessageList.Add(ex.Message + " - An error occur while getting employee details. Please try again.");
                return Ok(employeeDto.MessageList);
            }
            var result = await Task.FromResult(lstEmployeeDto);
            return Ok(result);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            //var result = await Task.FromResult(StaticEmployees.ResultList.FirstOrDefault(m => m.Id == id));
            employee_DA = new Employee_DA();
            employeeDto = new EmployeeDto();
            try
            {
                employeeDto.Id = id;
                employeeDto = employee_DA.Get(employeeDto);
            }
            catch (Exception ex)
            {
                employeeDto = new EmployeeDto();
                employeeDto.MessageList = new List<string>();
                employeeDto.MessageList.Add(ex.Message + " - An error occur while getting employee list. Please try again.");
                return Ok(employeeDto.MessageList);
            }
            var result = await Task.FromResult(employeeDto);
            return Ok(employeeDto);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and update changes to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(EmployeeDto input)
        {
            employee_DA = new Employee_DA();
            employeeDto = new EmployeeDto();
            employeeDto.Tin = input.Tin;
            employeeDto = employee_DA.Get(employeeDto);
            try
            {
                if (employeeDto != null && employeeDto.Id != input.Id)
                {
                    employeeDto.MessageList = new List<string>();
                    employeeDto.MessageList.Add("TIN already exists.");
                    return Ok(employeeDto.MessageList);
                }
                else
                {
                    employeeDto = new EmployeeDto();
                    employeeDto.isSuccess = employee_DA.Update(input);
                    employeeDto.MessageList = new List<string>();
                    if (!employeeDto.isSuccess)
                    {
                        employeeDto.MessageList.Add("Employee record was not updated. Please try again.");
                        return Ok(employeeDto.MessageList);
                    }

                    employeeDto.MessageList.Add("Employee successfully updated.");
                }
            }
            catch (Exception ex)
            {
                employeeDto = new EmployeeDto();
                employeeDto.MessageList = new List<string>();
                employeeDto.MessageList.Add(ex.Message + " - An error occur while updating employee details. Please try again.");
                return Ok(employeeDto.MessageList);
            }
            return Ok(employeeDto);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and insert employees to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(EmployeeDto input)
        {
            /// Check if TIN exists
            employee_DA = new Employee_DA();
            employeeDto = new EmployeeDto();
            try
            {
                employeeDto = employee_DA.Get(input);

                if (employeeDto != null)
                {
                    employeeDto.MessageList = new List<string>();
                    employeeDto.MessageList.Add("TIN already exists.");
                    return Ok(employeeDto.MessageList);
                }
                else
                {
                    employee_DA = new Employee_DA();
                    input = employee_DA.Insert(input);
                    if (!input.isSuccess)
                    {
                        employeeDto.MessageList = new List<string>();
                        employeeDto.MessageList.Add("Employee record was not inserted. Please try again.");
                        return Ok(employeeDto.MessageList);
                    }
                }
            }
            catch (Exception ex)
            {
                employeeDto = new EmployeeDto();
                employeeDto.MessageList = new List<string>();
                employeeDto.MessageList.Add(ex.Message + " - An error occur while updating employee details. Please try again.");
                return Ok(employeeDto.MessageList);
            }
            return Created($"/api/employees/{input.Id}", input.Id);
        }


        /// <summary>
        /// Refactor this method to go through proper layers and perform soft deletion of an employee to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            employee_DA = new Employee_DA();
            employeeDto = new EmployeeDto();
            try
            {
                employeeDto.Id = id;
                employeeDto.isDeleted = true;
                employeeDto.isSuccess = employee_DA.Delete(employeeDto);
                if (!employeeDto.isSuccess)
                {
                    employeeDto.MessageList = new List<string>();
                    employeeDto.MessageList.Add("Employee record was not deleted. Please try again.");
                    return Ok(employeeDto.MessageList);
                }
            }
            catch (Exception ex)
            {
                employeeDto = new EmployeeDto();
                employeeDto.MessageList = new List<string>();
                employeeDto.MessageList.Add(ex.Message + " - An error occur while deleting the employee. Please try again.");
                return Ok(employeeDto.MessageList);
            }
            return Ok(id);
        }



        /// <summary>
        /// Refactor this method to go through proper layers and use Factory pattern
        /// </summary>
        /// <param name="id"></param>
        /// <param name="absentDays"></param>
        /// <param name="workedDays"></param>
        /// <returns></returns>
        [HttpPost("{id}/calculate")]
        public async Task<IActionResult> Calculate(EmployeeDtoCalculate input)//(int id,int absentDays,int workedDays)
        {
            employee_DA = new Employee_DA();
            employeeDto = new EmployeeDto();
            decimal TotalDeduction = 0;
            decimal TotalAbsent = 0;
            decimal TaxDeduction = 0;
            decimal MonthlyRate = 20000.00m;
            decimal DailyRate = 500.00m;
            decimal RequiredDays = 22.0m;
            decimal Percentage = 0.12m;
            try
            {
                employeeDto.Id = input.Id;
                employeeDto = employee_DA.Get(employeeDto);
                
                if (employeeDto == null)
                {
                    employeeDto.MessageList.Add("Employee  not found.");
                    return Ok(employeeDto.MessageList);
                }

                var type = (EmployeeType)employeeDto.TypeId;

                switch (type)
                {
                    case EmployeeType.Regular:
                        TotalAbsent = (MonthlyRate / RequiredDays) * input.absentDays;
                        TaxDeduction = Percentage * MonthlyRate;
                        TotalDeduction = MonthlyRate - (TotalAbsent + TaxDeduction);
                        break;
                    case EmployeeType.Contractual:
                        TotalDeduction = input.workedDays * DailyRate;
                        break;
                    default:
                        employeeDto.MessageList = new List<string>();
                        employeeDto.MessageList.Add("Employee type not found.");
                        return Ok(employeeDto.MessageList);
                        break;
                }
            }
            catch (Exception ex)
            {
                employeeDto = new EmployeeDto();
                employeeDto.MessageList = new List<string>();
                employeeDto.MessageList.Add(ex.Message + " - An error occur while computing net income. Please try again.");
                return Ok(employeeDto.MessageList);
            }
            return Ok(TotalDeduction);
        }

        

    }
}
