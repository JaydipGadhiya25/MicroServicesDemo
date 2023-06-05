using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoginServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        public IConfiguration _configuration;
        public LoginController(IConfiguration config)
        {
            _configuration = config;
           
        }
        [HttpPost]
        [Route("Login")]

        public async Task<IActionResult> Login(string username,string password)
        {
            if (username != null && password != null)
            {
                if (username == "jaydip" && password == "jaydip123")
                {
                    //create claims details based on the user information
                    var claims = new[] {

                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserName",username),
                        
                        };
                

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);

                    // Store the token in the user's session
                    HttpContext.Session.SetString("JwtToken", new JwtSecurityTokenHandler().WriteToken(token));

                    if (token == null)
                    {
                        // Return a 500 Internal Server Error status code with an error message
                       return new JsonResult(new
                        {

                            message = "Failed to generate JWT token",
                            item = new JwtSecurityTokenHandler().WriteToken(token),
                            code = 500
                        });
                    }

                    return new JsonResult(new
                    {

                        message = "Succesfully Loggin",
                        item = new JwtSecurityTokenHandler().WriteToken(token),
                        code = 200
                    });
                }
                else
                {
                    return new JsonResult(new
                    {

                        message = "Invalid credential",
                        item = "No token generated",
                        code = 401
                    });
                }
            }
            else
            {
                return new JsonResult(new
                {

                    message = "Server can not process further due to client error",
                    item = "No token generated",
                    code = 400
                });
            }

        }
    }
}
