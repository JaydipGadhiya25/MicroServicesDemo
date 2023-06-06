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
            var query = "Select * from employee WHERE Id = @id";
            try
            {
                using (var connection = _context.CreateConnection())
                {

                    var employees = await connection.QueryAsync<EmployeeViewModel>(query, new { id });

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
        
        
        [HttpPost]
        [Route("EditEmployee")]
        [Authorize]
        public async Task<IActionResult> EditEmployee(EmployeeViewModel employeeData)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var p = new DynamicParameters();
                    p.Add("@id", employeeData.Id);
                    p.Add("@firstname", employeeData.First_Name);
                    p.Add("@lastname", employeeData.Last_Name);
                    p.Add("@employeeid", employeeData.Employee_Id);
                    p.Add("@city", employeeData.City);
                    p.Add("@departmentname", employeeData.Department_Name);
                    p.Add("@joiningdate", employeeData.Joining_Date);
                    p.Add("@createddate", DateTime.UtcNow);
                    p.Add("@updateddate", DateTime.UtcNow);

                    var employees = await connection.QueryAsync<EmployeeViewModel>("EditEmployee", p, commandType: CommandType.StoredProcedure);
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

        [HttpGet]
        [Route("DeleteEmployee")]
        [Authorize]
        public async Task<JsonResult> DeleteEmployee(int id)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {


                    var sqlStatement = "DELETE FROM employee WHERE id = @Id";
                    var employees = await connection.QueryAsync<EmployeeViewModel>(sqlStatement, new { id });
                    return new JsonResult(new
                    {

                        message = "Succesfully delete Employee",
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
