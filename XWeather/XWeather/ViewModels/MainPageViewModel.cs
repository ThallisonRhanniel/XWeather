using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using DryIoc;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Essentials;
using XWeather.Database;
using XWeather.Helpers;
using XWeather.Models;

namespace XWeather.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {

        #region Properties

        private string _animationFile = Animation.FilePermission;
        public string AnimationFile
        {
            get { return _animationFile; }
            set { SetProperty(ref _animationFile, value); }
        }

        private string _animationText = Animation.TextPermissionGps;
        public string AnimationText
        {
            get { return _animationText; }
            set { SetProperty(ref _animationText, value); }
        }

        private bool _animationIsVisible = true;
        public bool AnimationIsVisible
        {
            get { return _animationIsVisible; }
            set { SetProperty(ref _animationIsVisible, value); }
        }

        private bool _buttonOkayIsVisible = true;
        public bool ButtonOkayIsVisible
        {
            get { return _buttonOkayIsVisible; }
            set { SetProperty(ref _buttonOkayIsVisible, value); }
        }



        public ObservableCollection<HourlyWeather> HourlyWeatherList { get; set; }
        public ObservableCollection<HourlyWeather> BackupHourlyWeatherList { get; set; }


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

        private int _temperature;
        public int Temperature
        {
            get { return _temperature; }
            set { SetProperty(ref _temperature, value); }
        }

        private bool _isToday = true;
        public bool IsToday
        {
            get { return _isToday; }
            set { SetProperty(ref _isToday, value); }
        }

        private bool _isTomorrow;
        public bool IsTomorrow
        {
            get { return _isTomorrow; }
            set { SetProperty(ref _isTomorrow, value); }
        }

        private string _averageTemperature;
        public string AverageTemperature
        {
            get { return _averageTemperature; }
            set { SetProperty(ref _averageTemperature, value); }
        }

        //informações da parte inferior.
        private DateTime _sunriseTime;
        public DateTime SunriseTime
        {
            get { return _sunriseTime; }
            set { SetProperty(ref _sunriseTime, value); }
        }

        private DateTime _sunsetTime;
        public DateTime SunsetTime
        {
            get { return _sunsetTime; }
            set { SetProperty(ref _sunsetTime, value); }
        }

        private string _precipProbability;
        public string PrecipProbability
        {
            get { return _precipProbability; }
            set { SetProperty(ref _precipProbability, value); }
        }

        private string _humidity;
        public string Humidity
        {
            get { return _humidity; }
            set { SetProperty(ref _humidity, value); }
        }

        private string _windSpeed;
        public string WindSpeed
        {
            get { return _windSpeed; }
            set { SetProperty(ref _windSpeed, value); }
        }

        private string _apparentTemperature;
        public string ApparentTemperature
        {
            get { return _apparentTemperature; }
            set { SetProperty(ref _apparentTemperature, value); }
        }

        private string _precipIntensity;
        public string PrecipIntensity
        {
            get { return _precipIntensity; }
            set { SetProperty(ref _precipIntensity, value); }
        }

        private string _pressure;
        public string Pressure
        {
            get { return _pressure; }
            set { SetProperty(ref _pressure, value); }
        }

        private string _visibility;
        public string Visibility
        {
            get { return _visibility; }
            set { SetProperty(ref _visibility, value); }
        }

        private int _uvIndex;
        public int UvIndex
        {
            get { return _uvIndex; }
            set { SetProperty(ref _uvIndex, value); }
        }







        #endregion

        #region Commands

        public DelegateCommand<string> ChangeClimateListCommand { get; private set; }

        public DelegateCommand NextSevenDaysCommand { get; private set; }

        public DelegateCommand OkayCommand { get; private set; }

        #endregion

        #region Methods

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            //Se já tiver dados no bando de dados não precisa sincronizar de novo.
            var weatherOffline = await GetWeatherOfflineAsync();
            if (weatherOffline?.Currently != null)
            {
                AnimationIsVisible = false;
                await StartData();
            }
            else
            {
                if (!HasInternetConnetion())
                {
                    AnimationFile = Animation.FileWifi;
                    AnimationText = Animation.WithoutConnection;
                    return;
                }
            }
        }

        private async Task StartData()
        {
            //Primeira verifico se tem dados salvos de alguma consulta anterior
            var weatherOffline = await GetWeatherOfflineAsync();
            if (weatherOffline?.Currently != null) //
                FillScreenData(weatherOffline);
            else
            {
                //Pego os dados na API da WEB.
                var weatherApi = await GetWeatherAsync();
                await SaveWeatherOffline(weatherApi);
                FillScreenData(weatherApi);
            }
        }

        protected async Task<Location> GetPosition()
        {

            try
            {
                var location =
                     await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Default,
                         TimeSpan.FromSeconds(4)));

                if (location != null)
                    return location;
            }
            catch (FeatureNotSupportedException)
            {
                // Handle not supported on device exception
                Console.WriteLine("Esse dispostivo não tem suporte a GPS.");
            }
            catch (FeatureNotEnabledException)
            {
                // Handle not enabled on device exception

            }
            catch (PermissionException)
            {
                // Handle permission exception
                Console.WriteLine("Aplicativo não tem permissão para usar o GPS!");

            }
            catch (Exception)
            {
                Console.WriteLine("Incapaz de de Obter localização.");
            }
            return null;
        }




        private async Task<WeatherModel> GetWeatherAsync()
        {
            try
            {
                var api = GetApi();
                //Para chegar aqui é necessário conceder acesso ao GPS.
                var result = await GetPosition();
                var weather = await api.GetWeather(result.Latitude, result.Longitude);
                return weather;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
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


        private async Task SaveWeatherOffline(WeatherModel weather)
        {
            try
            {
                //Limpo os dados do banco para não ter duplicidade, e garantir que sempre terá os dados mais novos.
                await CleanDb();
                var database = new RepositoryDatabase();

                await database.SaveItem(weather.Currently);
                await database.SaveList(weather.Daily.Data, RepositoryDatabase.ActionDb.Insert);
                await database.SaveList(weather.Hourly.Data, RepositoryDatabase.ActionDb.Insert);
            }
            catch (Exception e)
            {
                Console.WriteLine("Não foi possível salvar os dados no SQLITE");
                Console.WriteLine(e.Message);

            }

        }



        private void FillScreenData(WeatherModel weather)
        {
            //Dados superior
            Icon = weather.Currently.Icon.Replace('-', '_');

            Date = UnixTimeStampToDateTime(weather.Currently.Time);
            Temperature = (int)weather.Currently.Temperature;
            //Backup da Lista do clima durante 24H
            foreach (var item in weather.Hourly.Data)
            {
                BackupHourlyWeatherList.Add(new HourlyWeather()
                {
                    //O replace é para o aplicativo encontrar as imagens corretamente, pois a api disponibiliza os nomes do sicones com o padrão "-".
                    Icon = item.Icon.Replace('-', '_'),
                    Time = UnixTimeStampToDateTime(item.Time),
                    Temperature = (int)item.Temperature
                });
            }

            //Preencher Lista do clima durante 24H
            FillClimateList();

            //Informações da parte inferior.
            AverageTemperature = $"Min: {(int)weather.Daily.Data[0].TemperatureMin}°    Max: {(int)weather.Daily.Data[0].TemperatureMax}°";

            SunriseTime = UnixTimeStampToDateTime(weather.Daily.Data[0].SunriseTime);
            SunsetTime = UnixTimeStampToDateTime(weather.Daily.Data[0].SunsetTime);

            //O Substring é para remoção do 0
            PrecipProbability = $"{(weather.Currently.PrecipProbability.ToString("n").Substring(2).StartsWith('0') ? $"{weather.Currently.PrecipProbability.ToString("n").Substring(3)}%" : $"{weather.Currently.PrecipProbability.ToString("n").Substring(2)}%")}";
            Humidity = $"{weather.Currently.Humidity.ToString("n").Substring(2)}%";

            //A multiplicação é para tranformar Milhas em KM/h
            WindSpeed = $"{(int)(weather.Currently.WindSpeed * 1.609)} km/h";
            ApparentTemperature = $"{(int)weather.Currently.ApparentTemperature}°";

            //Formula para transformar polegas em centimetros. Fonte: https://www.metric-conversions.org/pt-br/comprimento/polegadas-em-centimetros.htm
            PrecipIntensity = $"{Math.Round(weather.Currently.PrecipIntensity / 0.39370, 1)} cm";
            Pressure = $"{(int)weather.Currently.Pressure} hPa";

            //O método Round foi utilizado para diminuir as casas decimais
            Visibility = $"{Math.Round(weather.Currently.Visibility, 1)} km";
            UvIndex = weather.Currently.UvIndex;

        }

        private void FillClimateList()
        {
            HourlyWeatherList.Clear();
            //Estou usado ForEach porque no método "ADD" que é chamado o evento "property change".
            if (IsToday)
            {
                foreach (var hourlyWeather in BackupHourlyWeatherList.Take(25))
                    HourlyWeatherList.Add(hourlyWeather);
            }
            else if (IsTomorrow)
            {
                foreach (var hourlyWeather in BackupHourlyWeatherList.Skip(24).Take(25))
                    HourlyWeatherList.Add(hourlyWeather);
            }
        }

        private async Task CleanDb()
        {
            var database = new RepositoryDatabase();
            await database.DeleteAllItens<Currently>();
            await database.DeleteAllItens<DailyData>();
            await database.DeleteAllItens<HourlyData>();

        }


        protected async Task<bool> RequestIfPermissionAreValid()
        {
            if (await RequestPermissions(Permission.Storage) && await RequestPermissions(Permission.Location))
                return true;
            else
                return false;
            

        }


        private async Task<bool> RequestPermissions(Permission permission)
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
                if (status != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(permission);
                    //Verifica se a permissão foi concendida
                    if (results.ContainsKey(permission))
                        status = results[permission];
                }

                if (status == PermissionStatus.Granted)
                    return true;
                else if (status != PermissionStatus.Unknown)
                    return false;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            return false;
        }

        public bool HasInternetConnetion()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
                return true;
            else
                return false;
        }

        #endregion

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            HourlyWeatherList = new ObservableCollection<HourlyWeather>();
            BackupHourlyWeatherList = new ObservableCollection<HourlyWeather>();

            ChangeClimateListCommand = new DelegateCommand<string>(day =>
            {
                switch (day)
                {
                    case "today":
                        IsToday = true;
                        IsTomorrow = false;
                        FillClimateList();
                        break;
                    case "tomorrow":
                        IsToday = false;
                        IsTomorrow = true;
                        FillClimateList();
                        break;
                }
            });

            NextSevenDaysCommand = new DelegateCommand(async () =>
            {
                await NavigationService.NavigateAsync("SecondPage");
            });

            OkayCommand = new DelegateCommand(async () =>
            {

                if (!HasInternetConnetion())
                {
                    AnimationFile = Animation.FileWifi;
                    AnimationText = Animation.WithoutConnection;
                    return;
                }

                if (await RequestIfPermissionAreValid())
                {
                    //altero a animação
                    AnimationFile = Animation.LoadingFile;
                    AnimationText = Animation.TextLoading;
                    ButtonOkayIsVisible = false;
                    await StartData();
                    AnimationIsVisible = false;
                }
                else
                {
                    AnimationText = Animation.TextPermissionGps;
                    AnimationFile = Animation.FilePermission;
                    ButtonOkayIsVisible = true;
                }
            });
        }
    }
}
