using System;
using System.Collections.Generic;
using System.Text;

namespace XWeather.Models
{
    public class HourlyWeather
    {
        public DateTime Time { get; set; }
        public string Icon { get; set; }
        public int Temperature { get; set; }
    }
}
