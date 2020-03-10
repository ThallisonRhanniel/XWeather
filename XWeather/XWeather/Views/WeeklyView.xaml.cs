using Xamarin.Forms;

namespace XWeather.Views
{
    public partial class WeeklyView : ContentPage
    {
        public WeeklyView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
