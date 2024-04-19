// Controllers/WeatherController.cs

using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace FloridaMedianIncomeMVC.Controllers
{
    public class WeatherController : Controller
    {
        private readonly HttpClient _httpClient;

        public WeatherController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        // Serve the weather dashboard view
        public IActionResult Index()
        {
            return View();
        }

        // Fetch weather data based on the provided city name
        [HttpPost]
        public async Task<IActionResult> GetWeather(string city)
        {
            const string apiKey = "bd5e378503939ddaee76f12ad7a97608"; // Replace with your API key

            // Construct the API URL
            string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";

            try
            {
                // Send a GET request to the API
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                // If the request is successful, parse the JSON response
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON response using System.Text.Json
                    using var jsonDocument = JsonDocument.Parse(jsonResponse);
                    var data = jsonDocument.RootElement;

                    // Extract temperature and humidity from the response
                    double temperature = data.GetProperty("main").GetProperty("temp").GetDouble();
                    int humidity = data.GetProperty("main").GetProperty("humidity").GetInt32();

                    // Return the data as JSON
                    return Json(new { temperature, humidity });
                }
                else
                {
                    // Handle unsuccessful API request
                    return StatusCode((int)response.StatusCode, "Error fetching weather data");
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle exceptions related to HTTP requests
                return StatusCode(500, $"Error fetching weather data: {ex.Message}");
            }
        }
    }
}
