using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Refit;
using XWeather.Models;

namespace XWeather.Services
{
    public interface IRestApi
    {
        [Get("/183813cbaffd14171cf5df835f806262/{latitude},{longitude}?exclude=flags&lang=pt&units=auto")]
        Task<WeatherModel> GetWeather(double latitude, double longitude);

    }
}
