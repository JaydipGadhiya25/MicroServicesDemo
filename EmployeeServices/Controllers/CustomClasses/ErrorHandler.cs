using EmployeeServices.Context;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeServices.Controllers.CustomClasses
{
    public class ErrorHandler
    {
        public JsonResult HandleError(ErrorDetails error)
        {
            if (error.StatusCode == 200)
            {
                return new JsonResult(new
                {

                    message = error.Message,
                    item = error.Item,
                    code = error.StatusCode
                });
            }
            else if (error.StatusCode == 201)
            {
                return new JsonResult(new
                {

                    message = error.Message,
                    item = error.Item,
                    code = error.StatusCode
                });
            }
            else if (error.StatusCode == 404)
            {
                return new JsonResult(new
                {

                    message = error.Message,
                    item = error.Item,
                    code = error.StatusCode
                });
            }
            else if (error.StatusCode == 500)
            {
                return new JsonResult(new
                {

                    message = error.Message,
                    item = error.Item,
                    code = error.StatusCode
                });
            }
            else if (error.StatusCode == 401)
            {
                return new JsonResult(new
                {

                    message = error.Message,
                    item = error.Item,
                    code = error.StatusCode
                });
            }
            return new JsonResult(new
            {

                message = error.Message,
                item = error.Item,
                code = 500
            });
        }
    }
}
