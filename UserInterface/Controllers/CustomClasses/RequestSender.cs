using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using UserInterface.Models;

namespace UserInterface.Controllers.CustomClasses
{
    public class RequestSender
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly IOptions<MyAppSetting> appsetting;


        public RequestSender(IHttpClientFactory httpClientFactory, IOptions<MyAppSetting> app,IConfiguration configuration, HttpClient httpClient)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpClient = httpClient;
            appsetting = app;

        }

        public async Task<HttpResponseMessage> SendPostRequest(string endpoint, object requestData, string authToken)
        {
            try
            {
                // Set the base URL of your API gateway
                string apiGatewayBaseUrl = appsetting.Value.GatewayUrl;

                // Construct the API endpoint URL
                string apiUrl = $"{apiGatewayBaseUrl}/{endpoint}";

                // Serialize the request data to JSON
                string jsonData = JsonConvert.SerializeObject(requestData);

                // Create a StringContent with the JSON data
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");


                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);


                // Send a POST request to the API gateway
                HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, content);

                return response;
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                // ...
                throw;
            }
        }
    }
}
