using Dapper;
using EmployeeServices.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Data;

namespace EmployeeServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {

        public readonly DapperContext _context;

        public EmployeeController(DapperContext context)
        {
            
            _context = context;
            
        }
        [HttpGet]
        [Route("GetEmployees")]
        [Authorize]
        
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {

                    var employees = await connection.QueryAsync<EmployeeViewModel>("select * from employee");

                    if(employees.Count() == 0)
                    {
                        return new JsonResult(new
                        {

                            message = "Employee List not found",
                            item = "null",
                            code = 404
                        });
                    }
                     return new JsonResult(new
                        {

                            message = "Succesfully returning Employee List",
                            item = employees,
                            code = 201
                        });

                }
            }
            
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the retrieval of employee list
                return new JsonResult(new
                {

                    message = ex.Message,
                    item = "null",
                    code = 500
                });

            }
        }

        [HttpGet]
        [Route("GetOneEmployees")]
        [Authorize]

        public async Task<IActionResult> GetOneEmployees(int id)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {

                    var employees = await connection.QueryAsync<EmployeeViewModel>("select * from employee where id=@ID");

                    if (employees.Count() == 0)
                    {
                        return new JsonResult(new
                        {

                            message = "Employee List not found",
                            item = "null",
                            code = 404
                        });
                    }
                    return new JsonResult(new
                    {

                        message = "Succesfully returning Employee List",
                        item = employees,
                        code = 201
                    });

                }
            }

            catch (Exception ex)
            {
                // Handle any exceptions that occur during the retrieval of employee list
                return new JsonResult(new
                {

                    message = ex.Message,
                    item = "null",
                    code = 500
                });

            }
        }



        [HttpPost]
        [Route("CreateNewEmployee")]
        [Authorize]
        public async Task<IActionResult> CreateNewEmployee(EmployeeViewModel employeeData)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var p = new DynamicParameters();
                    p.Add("@firstname", employeeData.First_Name);
                    p.Add("@lastname", employeeData.Last_Name);
                    p.Add("@employeeid", employeeData.Employee_Id);
                    p.Add("@city", employeeData.City);
                    p.Add("@departmentname", employeeData.Department_Name);
                    p.Add("@joiningdate", employeeData.Joining_Date);
                    p.Add("@createddate", DateTime.UtcNow);

                    var employees = await connection.QueryAsync<EmployeeViewModel>("CreateNewEmployee", p, commandType: CommandType.StoredProcedure);
                    return new JsonResult(new
                    {

                        message = "Succesfully returning Employee List",
                        item = employees,
                        code = 201
                    });

                }


            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {

                    message = ex.Message,
                    item = "null",
                    code = 500
                });
            }
            
        }

    }
}
