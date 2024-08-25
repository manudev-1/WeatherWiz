using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using WeatherWiz.Models;

namespace WeatherWiz.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        // Attribute
        private readonly TimeZoneService tzService = new();
        private readonly WeatherService weatherService = new();
        private readonly WebView? _webView;
        private readonly System.Timers.Timer _timer;
        private string? _location;
        private TimeOnly? _time; 
        private Tuple<double?, double?>? _coords;
        private ImageSource? _imageSource;
        private WeatherResumeResponse? _weatherDay;
        private int _temp;
        private string? _descr;
        private int _highTemp;
        private int _lowTemp;

        // Property
        public string? Location
        {
            get => _location;
            set
            {
                if(SetProperty(ref _location, value))
                {
                    Task.Run(async () => 
                    {
                        Time = await tzService.GetTimeOnly(Coords?.Item1, Coords?.Item2);
                        WeatherDay = await weatherService.GetTodayResume(value.Split(",")[0]);
                    });
                }
            }
        }
        public TimeOnly? Time
        {
            get => _time;
            set 
            {
                if (SetProperty(ref _time, value))
                {
                    if (value.Value.Hour is >= 6 and < 8) ImageSource = "sunrise.jpg";
                    else if (value.Value.Hour is >= 8 and < 18) ImageSource = "day.jpg";
                    else if (value.Value.Hour is >= 18 and < 19) ImageSource = "sunset.jpg";
                    else ImageSource = "night.jpg";
                }
            }
        }
        public Tuple<double?,double?>? Coords
        {
            get { return _coords; }
            set { SetProperty(ref _coords, value); }
        }
        public ImageSource? ImageSource
        {
            get { return _imageSource; }
            set { SetProperty(ref _imageSource, value); }
        }
        public WeatherResumeResponse? WeatherDay
        {
            get { return _weatherDay; }
            set 
            { 
                if(SetProperty(ref _weatherDay, value))
                {
                    Temp = (int)value.Main.Temp;
                    
                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                    Description = textInfo.ToTitleCase(value.Weather[0].Description);

                    HigherTemp = (int)value.Main.Temp_max;
                    LowerTemp = (int)value.Main.Temp_min;
                } 
            }
        }
        public int Temp
        {
            get { return _temp; }
            set { SetProperty(ref _temp, value); }
        }
        public string? Description
        {
            get { return _descr; }
            set { SetProperty(ref _descr, value); }
        }
        public int HigherTemp
        {
            get { return _highTemp; }
            set { SetProperty(ref _highTemp, value); }
        }
        public int LowerTemp
        {
            get { return _lowTemp; }
            set { SetProperty(ref _lowTemp, value); }
        }

        // Method
        public MainPageViewModel()
        {
            Task.Run(async () => 
            {
                await GetCurrentLocationAsync();
                await UpdateTimeAsync();
            });
            _webView = new();

            _timer = new( 60 * 60 * 1000); 
            _timer.Elapsed += async (sender, e) => await UpdateTimeAsync();
            _timer.Start();
        }
        public async Task GetCurrentLocationAsync()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.High,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
                }

                Coords = Tuple.Create(location?.Latitude, location?.Longitude);

                if (location != null)
                {
                    var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                    var placemark = placemarks?.FirstOrDefault();

                    if (placemark != null) Location = $"{placemark.Locality}, {placemark.CountryName}";
                    else Location = "Posizione non trovata";
                }
                else Location = "Impossibile ottenere la posizione";
            }
            catch (Exception ex)
            {
                Location = $"Errore: {ex.Message}";
            }
        } // End GetCurrentLocationAsync
        private async Task UpdateTimeAsync()
        {
            var result = await _webView.EvaluateJavaScriptAsync($"typeof updateTime === 'function'");
            if (result == "true")
            {
                await _webView.EvaluateJavaScriptAsync($"updateTime('{Time.Value:HH:mm}')");
            }
            else
            {
                Debug.WriteLine("JavaScript function 'updateTime' is not defined.");
            }
        } // End UpdateTimeAsync
    } // End LocationViewModel
}
