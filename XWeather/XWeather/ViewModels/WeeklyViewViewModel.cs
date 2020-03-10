using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Navigation;
using XWeather.Database;
using XWeather.Models;

namespace XWeather.ViewModels
{
    public class WeeklyViewViewModel : ViewModelBase
    {

        #region Properties

        public ObservableCollection<WeeklyWeather> WeeklyWeatherList { get; set; }


        private string _icon;
        public string Icon
        {
            get { return _icon; }
            set { SetProperty(ref _icon, value); }
        }

        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        private int _temperatureMin;
        public int TemperatureMin
        {
            get { return _temperatureMin; }
            set { SetProperty(ref _temperatureMin, value); }
        }

        private int _temperatureMax;
        public int TemperatureMax
        {
            get { return _temperatureMax; }
            set { SetProperty(ref _temperatureMax, value); }
        }

        private string _windSpeed;
        public string WindSpeed
        {
            get { return _windSpeed; }
            set { SetProperty(ref _windSpeed, value); }
        }

        private string _visibility;
        public string Visibility
        {
            get { return _visibility; }
            set { SetProperty(ref _visibility, value); }
        }

        private string _humidity;
        public string Humidity
        {
            get { return _humidity; }
            set { SetProperty(ref _humidity, value); }
        }

        private int _uvIndex;
        public int UvIndex
        {
            get { return _uvIndex; }
            set { SetProperty(ref _uvIndex, value); }
        }


        #endregion

        #region Methods


        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            await StartData();

        }

        private async Task StartData()
        {
            var weatherOffline = await GetWeatherOfflineAsync();
            FillScreenData(weatherOffline);
        }

        private async Task<WeatherModel> GetWeatherOfflineAsync()
        {
            try
            {
                var database = new RepositoryDatabase();
                var currently = await database.GetAllItems<Currently>();
                var weather = new WeatherModel();
                weather.Daily = new Daily() { Data = new List<DailyData>(await database.GetAllItems<DailyData>()) };
                weather.Hourly = new Hourly() { Data = new List<HourlyData>(await database.GetAllItems<HourlyData>()) };
                weather.Currently = currently.FirstOrDefault();

                return weather;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        private void FillScreenData(WeatherModel weather)
        {
            //O replace é para o aplicativo encontrar as imagens corretamente, pois a api disponibiliza os nomes do sicones com o padrão "-".
            Icon = weather.Currently.Icon.Replace('-', '_');

            Date = UnixTimeStampToDateTime(weather.Currently.Time);

            TemperatureMin = (int)weather.Daily.Data.First().TemperatureMin;
            TemperatureMax = (int)weather.Daily.Data.First().TemperatureMax;

            WindSpeed = $"{(int)(weather.Currently.WindSpeed * 1.609)} km/h";

            Visibility = $"{Math.Round(weather.Currently.Visibility, 1)} km";

            Humidity = $"{weather.Currently.Humidity.ToString("n").Substring(2)}%";

            UvIndex = weather.Currently.UvIndex;

            foreach (var item in weather.Daily.Data.Skip(1).Take(6))
            {
                WeeklyWeatherList.Add(new WeeklyWeather()
                {
                    //O replace é para o aplicativo encontrar as imagens corretamente, pois a api disponibiliza os nomes do sicones com o padrão "-".
                    Icon = item.Icon.Replace('-', '_'),
                    Time = UnixTimeStampToDateTime(item.Time),
                    PrecipProbability = $"{(item.PrecipProbability.ToString("n").Substring(2).StartsWith('0') ? $"{item.PrecipProbability.ToString("n").Substring(3)}%" : $"{item.PrecipProbability.ToString("n").Substring(2)}%")}",
                    TemperatureMin = (int)item.TemperatureMin,
                    TemperatureMax = (int)item.TemperatureMax,
                });
            }
        }

        #endregion

        public WeeklyViewViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            WeeklyWeatherList = new ObservableCollection<WeeklyWeather>();
        }
    }
}
