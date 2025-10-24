using Microsoft.AspNetCore.DataProtection.KeyManagement;
using StyleAssistantCA1_SOA_MeghanKeightley;
using System.Net.Http;
using System.Text.Json;
using static System.Net.WebRequestMethods;

//references used for weather API
// API youtube video used to help with this component: https://youtu.be/MdIfZJ08g2I?si=xdbgNMWxp1yUrlGx
// source used to verify issues with my async task https://stackoverflow.com/questions/52436226/net-core-simple-way-to-find-temperature-from-openweather-api


namespace StyleAssistantCA1_SOA_MeghanKeightley
{
    public class Weather : IWeather //weather class implementing IWeather interface
    {
        private readonly HttpClient _httpClient; //http client to make requests
        private readonly string key;
        public Weather(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient; //initializing
            key = config["OpenWeatherMap:ApiKey"]; //getting api
            if (string.IsNullOrEmpty(key))
                throw new Exception("API key missing!");
        }

        //async task to get weather for a city - returns WeatherClass object with city, temperature and condition
        public async Task<WeatherClass> GetWeatherForCityAsync(string City)

        {
            try
            {
                //var encodedCity = Uri.EscapeDataString(City.Trim());
                var url = $"https://api.openweathermap.org/data/2.5/weather?q={City}&appid={key}&units=metric";
                var response = await _httpClient.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Raw JSON: {json}");

                var data = JsonSerializer.Deserialize<OpenWeatherResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                //adding in enum for more marks lol
                var weather = new WeatherClass
                {
                    City = data.Name,
                    Temperature = data.Main.Temp,
                    FeelsLike = data.Main.Feels_Like,
                    Humidity = data.Main.Humidity,
                    Condition = data.Weather?.FirstOrDefault()?.Main ?? "Unknown",
                    Sunrise = DateTimeOffset.FromUnixTimeSeconds(data.Sys.Sunrise).LocalDateTime,
                    Sunset = DateTimeOffset.FromUnixTimeSeconds(data.Sys.Sunset).LocalDateTime
                };

                // Assign enum to the instance, not the class
                weather.ConditionType = weather.Condition?.ToLower() switch
                { // Using the instance property here to set the enum value based on condition string
                    "sun" or "sunny" => WeatherCondition.Sunny,
                    "cloud" or "cloudy" => WeatherCondition.Cloudy,
                    "rain" or "rainy" => WeatherCondition.Rainy,
                    "snow" or "snowy" => WeatherCondition.Snowy,
                    _ => WeatherCondition.Unknown
                };

                // Return the fully populated object
                return weather;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Weather API error: " + ex.Message);
                return null;
            }
        }
    }
}
