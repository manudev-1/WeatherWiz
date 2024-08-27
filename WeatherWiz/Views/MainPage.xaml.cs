using System.Collections.ObjectModel;
using System.Diagnostics;
using WeatherWiz.Models;
using WeatherWiz.ViewModels;

namespace WeatherWiz
{
    public partial class MainPage : ContentPage
    {
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
    } // End class
} // End namespace
