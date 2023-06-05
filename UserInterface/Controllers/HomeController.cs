using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using UserInterface.Models;

namespace UserInterface.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        private readonly IOptions<MyAppSetting> appsetting;

        public HomeController(ILogger<HomeController> logger, HttpClient httpClient,IOptions<MyAppSetting> app)
        {
            _logger = logger;
            _httpClient = httpClient;
            appsetting = app;
        }

        [HttpPost]
        public IActionResult SetSession(string token)
        {
            // Store the token value in the session
            HttpContext.Session.SetString("Token", token);

            // Return a response indicating success
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
        public async Task<IActionResult> editEmployeeAsync()
        {
            try
            {


                // Set the base URL of your API gateway
                string apiGatewayBaseUrl = appsetting.Value.GatewayUrl;

                // Construct the API endpoint URL
                string apiUrl = $"{apiGatewayBaseUrl}/GetOneEmployee";

               

                // Retrieve the token from the user's session
                string jwtToken = HttpContext.Session.GetString("Token");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);


                // Send a POST request to the API gateway
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
               

                // Check the response status code
                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();

                    dynamic responseData = JsonConvert.DeserializeObject(jsonString);

                    var itemsArray = responseData.item;

                    // Convert the items array into a list of EmployeeViewModel
                    EmployeeViewModel employees = itemsArray.ToObject<EmployeeViewModel>();
                    // Deserialize the JSON string into the model
                    //var jsonData = JsonSerializer.Deserialize<List<EmployeeViewModel>>(jsonString);

                    // Successful API call
                    return View(employees);
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
        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> CreateNewEmployeeAsync(EmployeeViewModel empData)
        {
            try
            {
               

                // Set the base URL of your API gateway
                string apiGatewayBaseUrl = appsetting.Value.GatewayUrl;

                // Construct the API endpoint URL
                string apiUrl = $"{apiGatewayBaseUrl}/newEmployee";

                // Serialize the employee data to JSON
                string jsonData = JsonConvert.SerializeObject(empData);

                // Create a StringContent with the JSON data
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Retrieve the token from the user's session
                string jwtToken = HttpContext.Session.GetString("Token");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);


                // Send a POST request to the API gateway
                HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, content);

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