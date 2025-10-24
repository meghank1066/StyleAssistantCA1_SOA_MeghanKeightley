
namespace StyleAssistantCA1_SOA_MeghanKeightley
{

    public interface IWeather
    {
        Task<WeatherClass> GetWeatherForCityAsync(string City);
    }


    public class WeatherClass
    {
        public string City { get; set; }
        public double Temperature { get; set; }
        public string Condition { get; set; }

        public double FeelsLike { get; set; }  
        public int Humidity { get; set; } 
        public DateTime Sunrise { get; set; }     
        public DateTime Sunset { get; set; }   
    }
    public class MainInfo
    {
        public double Temp { get; set; }
        public double Feels_Like { get; set; }
        public int Humidity { get; set; }
    }
    public class SysInfo
    {
        public long Sunrise { get; set; }
        public long Sunset { get; set; }
    }

    public class OpenWeatherResponse
    {
        public string Name { get; set; }
        public MainInfo Main { get; set; } 
        public List<WeatherDescription> Weather { get; set; }
        public SysInfo Sys { get; set; }
    }


    public class WeatherDescription
    {
        public string Main { get; set; }
        public string Description { get; set; }
    }

}


