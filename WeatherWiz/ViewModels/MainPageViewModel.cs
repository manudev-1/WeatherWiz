using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows.Input;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using WeatherWiz.Models;
using WeatherWiz.Util;

namespace WeatherWiz.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private string _location;

        public TimeViewModel TimeViewModel { get; private set; }
        public WeatherViewModel WeatherViewModel { get; private set; }
        public UIStateViewModel UIStateViewModel { get; private set; }
        public string Location
        {
            get { return _location; }
            set { SetProperty(ref _location, value); }
        }

        public MainPageViewModel()
        {
            TimeViewModel = new TimeViewModel();
            WeatherViewModel = new WeatherViewModel();
            UIStateViewModel = new UIStateViewModel();

            var app = (App)Application.Current;
            if (app == null) return;

            app.CurrentLocationUpdated += App_CurrentLocationUpdated;
        } // End Constructor
        private async void App_CurrentLocationUpdated(CurrentLocation obj)
        {
            Location = obj.LocationResult ?? "";
        } // End App_LocationUpdated
    } // End MainPageViewModel
}
