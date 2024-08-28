using System.Collections.ObjectModel;
using System.Diagnostics;
using WeatherWiz.Models;
using WeatherWiz.Util;
using WeatherWiz.ViewModels;

namespace WeatherWiz
{
    public partial class MainPage : ContentPage
    {
        public string? CityName { get; set; }
        public MainPage()
        {
            InitializeComponent();
            webView.Source = new UrlWebViewSource
            {
                Url = "index.html"
            };
            Task.Run(async () => 
            { 
                var resp = await Helper.GetCurrentLocationAsync();
                CityName = resp.LocationResult;
            });
        } // End Constructor
        private async void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            var viewModel = (MainPageViewModel)BindingContext;
            await viewModel.PanUpdate(new() { EventArgs = e, Sender = sender }); 
        } // End PanGestureRecognizer_PanUpdated
        private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            await Launcher.OpenAsync($"https://www.meteoblue.com/en/weather/outdoorsports/airquality/{CityName}");
        } // End TapGestureRecognizer_Tapped
    } // End class
} // End namespace
