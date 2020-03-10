using System.Collections.Generic;
using Newtonsoft.Json;
using SQLite;

namespace XWeather.Models
{
    public class WeatherModel
    {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
        [JsonProperty("longitude")]
        public double Longitude { get; set; }
        [JsonProperty("timezone")]
        public string TimeZone { get; set; }
        [JsonProperty("currently")]
        public Currently Currently { get; set; }
        [JsonProperty("hourly")]
        public Hourly Hourly { get; set; }
        [JsonProperty("daily")]
        public Daily Daily { get; set; }
        [JsonProperty("offset")]
        public int Offset { get; set; }
    }


    public class Currently
    {
        [JsonIgnore()]
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
        [JsonProperty("time")]
        public int Time { get; set; }
        [JsonProperty("summary")]
        public string Summary { get; set; }
        [JsonProperty("icon")]
        public string Icon { get; set; }
        [JsonProperty("precipIntensity")]
        public double PrecipIntensity { get; set; }
        [JsonProperty("precipProbability")]
        public double PrecipProbability { get; set; }
        [JsonProperty("temperature")]
        public double Temperature { get; set; }
        [JsonProperty("apparentTemperature")]
        public double ApparentTemperature { get; set; }
        [JsonProperty("dewPoint")]
        public double DewPoint { get; set; }
        [JsonProperty("humidity")]
        public double Humidity { get; set; }
        [JsonProperty("pressure")]
        public double Pressure { get; set; }
        [JsonProperty("windSpeed")]
        public double WindSpeed { get; set; }
        [JsonProperty("windGust")]
        public double WindGust { get; set; }
        [JsonProperty("windBearing")]
        public int WindBearing { get; set; }
        [JsonProperty("cloudCover")]
        public double CloudCover { get; set; }
        [JsonProperty("uvIndex")]
        public int UvIndex { get; set; }
        [JsonProperty("visibility")]
        public double Visibility { get; set; }
        [JsonProperty("ozone")]
        public double Ozone { get; set; }
    }


    public class Hourly
    {
        [JsonProperty("summary")]
        public string Summary { get; set; }
        [JsonProperty("icon")]
        public string Icon { get; set; }
        [JsonProperty("data")]
        public List<HourlyData> Data { get; set; }
    }

    public class Daily
    {
        [JsonProperty("summary")]
        public string Summary { get; set; }
        [JsonProperty("icon")]
        public string Icon { get; set; }
        [JsonProperty("data")]
        public List<DailyData> Data { get; set; }
    }

    public class HourlyData
    {
        [JsonIgnore()]
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
        [JsonProperty("time")]
        public int Time { get; set; }
        [JsonProperty("summary")]
        public string Summary { get; set; }
        [JsonProperty("icon")]
        public string Icon { get; set; }
        [JsonProperty("precipIntensity")]
        public double PrecipIntensity { get; set; }
        [JsonProperty("precipProbability")]
        public double PrecipProbability { get; set; }
        [JsonProperty("temperature")]
        public double Temperature { get; set; }
        [JsonProperty("apparentTemperature")]
        public double ApparentTemperature { get; set; }
        [JsonProperty("dewPoint")]
        public double DewPoint { get; set; }
        [JsonProperty("humidity")]
        public double Humidity { get; set; }
        [JsonProperty("pressure")]
        public double Pressure { get; set; }
        [JsonProperty("windSpeed")]
        public double WindSpeed { get; set; }
        [JsonProperty("windGust")]
        public double WindGust { get; set; }
        [JsonProperty("windBearing")]
        public int WindBearing { get; set; }
        [JsonProperty("cloudCover")]
        public double CloudCover { get; set; }
        [JsonProperty("uvIndex")]
        public int UvIndex { get; set; }
        [JsonProperty("visibility")]
        public double Visibility { get; set; }
        [JsonProperty("ozone")]
        public double Ozone { get; set; }
        [JsonProperty("precipType")]
        public string PrecipType { get; set; }
    }

    public class DailyData
    {
        [JsonIgnore()]
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
        [JsonProperty("time")]
        public int Time { get; set; }
        [JsonProperty("summary")]
        public string Summary { get; set; }
        [JsonProperty("icon")]
        public string Icon { get; set; }
        [JsonProperty("sunriseTime")]
        public int SunriseTime { get; set; }
        [JsonProperty("sunsetTime")]
        public int SunsetTime { get; set; }
        [JsonProperty("moonPhase")]
        public double MoonPhase { get; set; }
        [JsonProperty("precipIntensity")]
        public double PrecipIntensity { get; set; }
        [JsonProperty("precipIntensityMax")]
        public double PrecipIntensityMax { get; set; }
        [JsonProperty("precipIntensityMaxTime")]
        public int PrecipIntensityMaxTime { get; set; }
        [JsonProperty("precipProbability")]
        public double PrecipProbability { get; set; }
        [JsonProperty("precipType")]
        public string PrecipType { get; set; }
        [JsonProperty("temperatureHigh")]
        public double TemperatureHigh { get; set; }
        [JsonProperty("temperatureHighTime")]
        public int TemperatureHighTime { get; set; }
        [JsonProperty("temperatureLow")]
        public double TemperatureLow { get; set; }
        [JsonProperty("temperatureLowTime")]
        public double TemperatureLowTime { get; set; }
        [JsonProperty("apparentTemperatureHigh")]
        public double ApparentTemperatureHigh { get; set; }
        [JsonProperty("apparentTemperatureHighTime")]
        public double ApparentTemperatureHighTime { get; set; }
        [JsonProperty("apparentTemperatureLow")]
        public double ApparentTemperatureLow { get; set; }
        [JsonProperty("apparentTemperatureLowTime")]
        public int ApparentTemperatureLowTime { get; set; }
        [JsonProperty("dewPoint")]
        public double DewPoint { get; set; }
        [JsonProperty("humidity")]
        public double Humidity { get; set; }
        [JsonProperty("pressure")]
        public double Pressure { get; set; }
        [JsonProperty("windSpeed")]
        public double WindSpeed { get; set; }
        [JsonProperty("windGust")]
        public double WindGust { get; set; }
        [JsonProperty("windGustTime")]
        public int WindGustTime { get; set; }
        [JsonProperty("windBearing")]
        public int WindBearing { get; set; }
        [JsonProperty("cloudCover")]
        public double CloudCover { get; set; }
        [JsonProperty("uvIndex")]
        public int UvIndex { get; set; }
        [JsonProperty("uvIndexTime")]
        public int UvIndexTime { get; set; }
        [JsonProperty("visibility")]
        public double Visibility { get; set; }
        [JsonProperty("ozone")]
        public double Ozone { get; set; }
        [JsonProperty("temperatureMin")]
        public double TemperatureMin { get; set; }
        [JsonProperty("temperatureMinTime")]
        public int TemperatureMinTime { get; set; }
        [JsonProperty("temperatureMax")]
        public double TemperatureMax { get; set; }
        [JsonProperty("temperatureMaxTime")]
        public int TemperatureMaxTime { get; set; }
        [JsonProperty("apparentTemperatureMin")]
        public double ApparentTemperatureMin { get; set; }
        [JsonProperty("apparentTemperatureMinTime")]
        public int ApparentTemperatureMinTime { get; set; }
        [JsonProperty("apparentTemperatureMax")]
        public double ApparentTemperatureMax { get; set; }
        [JsonProperty("apparentTemperatureMaxTime")]
        public int ApparentTemperatureMaxTime { get; set; }
    }
}
