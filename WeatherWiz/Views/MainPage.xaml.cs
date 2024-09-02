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
        private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
        {

        } // End TapGestureRecognizer_Tapped_1
        private void TapGestureRecognizer_Tapped_2(object sender, TappedEventArgs e)
        {

        } // End TapGestureRecognizer_Tapped_2
    } // End class
} // End namespace
