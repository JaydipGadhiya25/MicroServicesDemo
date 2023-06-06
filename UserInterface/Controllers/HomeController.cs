using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using UserInterface.Controllers.CustomClasses;
using UserInterface.Models;

namespace UserInterface.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        private readonly IOptions<MyAppSetting> appsetting;
        private readonly RequestSender _apiGatewayService;

        public HomeController(ILogger<HomeController> logger,RequestSender apiGatewayService, HttpClient httpClient,IOptions<MyAppSetting> app)
        {
            _logger = logger;
            _httpClient = httpClient;
            appsetting = app;
            _apiGatewayService = apiGatewayService;
        }

        [HttpPost]
        public IActionResult setsession(string token)
        {
            // store the token value in the session
            HttpContext.Session.SetString("token", token);

            // return a response indicating success
            return Ok();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult CreateEmployee()
        {
            return View();
        }
       
        public async Task<IActionResult> editEmployee(int id)
        {
            ViewData["id"] = id;    
           return View();
               
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewEmployee(EmployeeViewModel empData)
        {
            try
            {
                string jwtToken = Request.Headers["RequestVerificationToken"];
                // Send a POST request to the API gateway using the service
                HttpResponseMessage response = await _apiGatewayService.SendPostRequest("newEmployee", empData, jwtToken);

               
                if (response.IsSuccessStatusCode)
                {
                    // Successful API call
                    return RedirectToAction("Dashboard", "Home");
                }
                else
                {
                    // Handle the error response
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    return RedirectToAction("Error", "Home", new { message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the API call
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> EditEmployeeMethod(EmployeeViewModel empData)
        {
            try
            {
                string jwtToken = Request.Headers["RequestVerificationToken"];
                // Send a POST request to the API gateway using the service
                HttpResponseMessage response = await _apiGatewayService.SendPostRequest("EditEmployee", empData, jwtToken);


                // Check the response status code
                if (response.IsSuccessStatusCode)
                {
                    // Successful API call
                    return RedirectToAction("Dashboard", "Home");
                }
                else
                {
                    // Handle the error response
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    return RedirectToAction("Error", "Home", new { message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the API call
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }
        }


       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}