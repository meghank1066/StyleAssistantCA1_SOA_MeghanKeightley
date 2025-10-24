using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

//references used for ASOS API
//https://rapidapi.com/apidojo/api/Asos


namespace StyleAssistantCA1_SOA_MeghanKeightley
{
    public class Asos
    {

        private readonly HttpClient _httpClient;
        private readonly string _asosKey;


        public Asos(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _asosKey = config["RapidAPIAsos:asosKey"];
            if (string.IsNullOrEmpty(_asosKey))
                throw new Exception("API key missing!");
        }

        //method which helps us to return our ASOS items based on weather info :)
        //    public async Task<List<StyleItem>> GetStyleForWeatherAsync(WeatherClass weather)
        //    {
        //        try
        //        {
        //            var season = GetSeason(DateTime.Now); // pick season
        //            var categories = season switch
        //            {
        //                "winter" => SeasonalCatalog.Winter(),
        //                "spring" => SeasonalCatalog.Spring(),
        //                "summer" => SeasonalCatalog.Summer(),
        //                "autumn" => SeasonalCatalog.Fall(),
        //                _ => new List<string>()
        //            };

        //            string cond = weather.Condition?.ToLower() ?? "";
        //            if (cond.Contains("rain"))
        //                categories.AddRange(new[] { "raincoats", "waterproof jackets", "umbrellas" });
        //            else if (cond.Contains("snow"))
        //                categories.AddRange(new[] { "snow boots", "thermal wear", "wool hats" });

        //            var allItems = new List<StyleItem>();


        //            foreach (var term in categories.Distinct())
        //            {
        //                int offset = 0;
        //                string encodedTerm = Uri.EscapeDataString(term);
        //                string url = $"https://asos2.p.rapidapi.com/products/v2/list?store=US&limit=50&offset={offset}&country=US&sort=freshness&currency=USD&q={encodedTerm}";


        //                var request = new HttpRequestMessage(HttpMethod.Get, url);
        //                request.Headers.Add("X-RapidAPI-Key", _asosKey);
        //                request.Headers.Add("X-RapidAPI-Host", "asos2.p.rapidapi.com");

        //                var response = await _httpClient.SendAsync(request);
        //                if (!response.IsSuccessStatusCode) continue;


        //                var json = await response.Content.ReadAsStringAsync();
        //                Console.WriteLine(json);
        //                var data = JsonSerializer.Deserialize<AsosApiResponse>(json, new JsonSerializerOptions
        //                {
        //                    PropertyNameCaseInsensitive = true
        //                });

        //                if (data?.Products == null || data.Products.Count == 0)
        //                    continue;
        //                Console.WriteLine($"Found {data.Products.Count} products for {term}");

        //                allItems.AddRange(data.Products.Select(p => new StyleItem
        //                {
        //                    Name = p.Name,
        //                    Brand = p.Brand?.Name,
        //                    Price = p.Price?.Current?.Text,
        //                    ImageUrl = !string.IsNullOrEmpty(p.ImageUrl)
        //? (p.ImageUrl.StartsWith("http") ? p.ImageUrl : $"https://{p.ImageUrl}")
        //: "",

        //                    ProductUrl = !string.IsNullOrEmpty(p.ProductUrl)
        //? (p.ProductUrl.StartsWith("http") ? p.ProductUrl : $"https://{p.ProductUrl}")
        //: "",

        //                }));
        //            }

        //            // Remove duplicates by Name
        //            return allItems.GroupBy(x => x.Name).Select(g => g.First()).ToList();
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("There has been an error with Asos :(: " + ex.Message);
        //            return new List<StyleItem>();
        //        }
        //    }

        public async Task<List<StyleItem>> GetStyleForWeatherAsync(WeatherClass weather)
        {
            try
            {
                List<string> categories;

                if (weather.Temperature <= 5)                 // Freezing/cold
                    categories = SeasonalCatalog.Winter();
                else if (weather.Temperature <= 18)           // Cool/mild
                    categories = SeasonalCatalog.Fall();
                else                                           // Hot/warm
                    categories = SeasonalCatalog.Summer();

                string cond = weather.Condition?.ToLower() ?? "";
                if (cond.Contains("rain"))
                    categories.AddRange(new[] { "raincoats", "waterproof jackets", "umbrellas" });
                else if (cond.Contains("snow"))
                    categories.AddRange(new[] { "snow boots", "thermal wear", "wool hats" });

                var allItems = new List<StyleItem>();

                foreach (var term in categories.Distinct())
                {
                    int offset = 0;
                    string encodedTerm = Uri.EscapeDataString(term);
                    string url = $"https://asos2.p.rapidapi.com/products/v2/list?store=US&limit=50&offset={offset}&country=US&sort=freshness&currency=USD&q={encodedTerm}";

                    var request = new HttpRequestMessage(HttpMethod.Get, url);
                    request.Headers.Add("X-RapidAPI-Key", _asosKey);
                    request.Headers.Add("X-RapidAPI-Host", "asos2.p.rapidapi.com");

                    var response = await _httpClient.SendAsync(request);
                    if (!response.IsSuccessStatusCode) continue;

                    var json = await response.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<AsosApiResponse>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (data?.Products == null || data.Products.Count == 0) continue;

                    allItems.AddRange(data.Products.Select(p => new StyleItem
                    {
                        Name = p.Name ?? "",
                        Brand = p.Brand?.Name ?? "",
                        Colour = p.Colour ?? "",   
                        Price = p.Price?.Current?.Text != null && p.Price.Current.Text != ""
        ? p.Price.Current.Text
        : "Price not available",

                        ImageUrl = !string.IsNullOrEmpty(p.ImageUrl)
                            ? (p.ImageUrl.StartsWith("http") ? p.ImageUrl : $"https://{p.ImageUrl}")
                            : "",
                        ProductUrl = !string.IsNullOrEmpty(p.ProductUrl)
                            ? (p.ProductUrl.StartsWith("http") ? p.ProductUrl : $"https://{p.ProductUrl}")
                            : ""
                    }));
                }

                // Remove duplicates by Name
                //returns a list of unique style items based on their names
                return allItems.GroupBy(x => x.Name).Select(g => g.First()).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("There has been an error with Asos :(: " + ex.Message);
                return new List<StyleItem>();
            }
        }


        private string GetSeason(DateTime date)
        {
            int month = date.Month;
            if (month >= 3 && month <= 5) return "spring";
            if (month >= 6 && month <= 8) return "summer";
            if (month >= 9 && month <= 11) return "autumn";
            return "winter";
        }
    }
}