using Dapper;
using EmployeeServices.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

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

                    var employees = await connection.QueryAsync<EmployeeViewModel>("seect * from employee");

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

    }
}
