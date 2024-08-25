using System.Diagnostics;
using WeatherWiz.ViewModels;

namespace WeatherWiz
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel? ViewModel => BindingContext as MainPageViewModel;
        public MainPage()
        {
            InitializeComponent();
            webView.Source = new UrlWebViewSource
            {
                Url = "index.html"
            };
        }
    }
}
