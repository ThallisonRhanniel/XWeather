using Prism.Mvvm;
using Prism.Navigation;
using System;
using Refit;
using XWeather.Helpers;
using XWeather.Services;

namespace XWeather.ViewModels
{
    public class ViewModelBase : BindableBase, IInitialize, INavigationAware, IDestructible
    {

        #region Properties
        protected INavigationService NavigationService { get; private set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }


        #endregion

        #region Methods

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix Epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public IRestApi GetApi() => RestService.For<IRestApi>(EndPoints.BaseUrl);

        #endregion



        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public virtual void Initialize(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {

        }
    }
}
