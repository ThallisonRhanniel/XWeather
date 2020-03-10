using System;


namespace XWeather.Models
{
    public class WeeklyWeather
    {
        public string Icon { get; set; }
        public DateTime Time { get; set; }
        public string PrecipProbability { get; set; }
        public int TemperatureMin { get; set; }
        public int TemperatureMax { get; set; }
    }
}

