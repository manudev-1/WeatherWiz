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
    public class EventParams
    {
        public object? Sender { get; set; }
        public PanUpdatedEventArgs? EventArgs { get; set; }
    }
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
        private WeatherFiveDaysResponse? _weatherWeek;
        private int _temp;
        private string? _descr;
        private int _highTemp;
        private int _lowTemp;
        private int _translationY;
        private bool _opened;

        // Property
        public ObservableCollection<object> Forecasts { get; set; } = new();
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
                        WeatherDay = await weatherService.GetTodayResume(value?.Split(",")[0] ?? "");
                        WeatherWeek = await weatherService.GetFiveDays(value?.Split(",")[0] ?? "");
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
#pragma warning disable CS8629 // Nullable value type may be null.
                    if (value.Value.Hour is >= 6 and < 8) ImageSource = "sunrise.jpg";
                    else if (value.Value.Hour is >= 8 and < 18) ImageSource = "day.jpg";
                    else if (value.Value.Hour is >= 18 and < 19) ImageSource = "sunset.jpg";
                    else ImageSource = "night.jpg";
#pragma warning restore CS8629 // Nullable value type may be null.
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
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    Temp = (int)value.Main.Temp;
                    HigherTemp = (int)value.Main.Temp_max;
                    LowerTemp = (int)value.Main.Temp_min;

                    Forecasts.Add(new WeatherForecast() { Time = "Now", Image = $"https://openweathermap.org/img/wn/{value?.Weather?[0].Icon}@2x.png", Temperature = (int)value.Main.Temp });
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                    Description = textInfo.ToTitleCase(value?.Weather?[0].Description ?? "");
                } 
            }
        }
        public WeatherFiveDaysResponse? WeatherWeek
        {
            get { return _weatherWeek; }
            set 
            {
                DateTime? previousDate = null;
                if (SetProperty(ref _weatherWeek, value))
                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    value.List.ForEach(el =>
                    {
#pragma warning disable CS8629 // Nullable value type may be null.
                        DateTime currentDate = el.Dt_txt.Value.Date;
#pragma warning restore CS8629 // Nullable value type may be null.

                        if (previousDate.HasValue && previousDate.Value != currentDate) Forecasts.Add(new Separator());

                        Forecasts.Add(new WeatherForecast()
                        {
                            Time = el.Dt_txt.HasValue ? el.Dt_txt.Value.ToString("H tt") : "",
                            Image = $"https://openweathermap.org/img/wn/{el.Weather?[0].Icon}@2x.png",
                            Temperature = (int)el.Main.Temp
                        });
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                        previousDate = currentDate;
                    });
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
        public int TraslationY
        {
            get { return _translationY; }
            set { SetProperty(ref _translationY, value); }
        }
        public bool Opened
        {
            get { return _opened; }
            set { SetProperty(ref _opened, value); }
        }

        // Method
        public MainPageViewModel()
        {
            Task.Run(async () => 
            {
                CurrentLocation? resp = await Helper.GetCurrentLocationAsync();
                Coords = resp?.Coords;
                Location = resp?.LocationResult;
                await UpdateTimeAsync();
            });
            _webView = new();

            _timer = new( 60 * 60 * 1000); 
            _timer.Elapsed += async (sender, e) => await UpdateTimeAsync();
            _timer.Start();

            TraslationY = 650;
        }
        private async Task UpdateTimeAsync()
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8629 // Nullable value type may be null.
            await _webView.EvaluateJavaScriptAsync($"updateTime('{Time.Value:HH:mm}')");
#pragma warning restore CS8629 // Nullable value type may be null.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        } // End UpdateTimeAsync
        public async Task PanUpdate(EventParams e)
        {
            var border = e.Sender as Border;
            switch (e.EventArgs?.StatusType)
            {
                case GestureStatus.Started:
                case GestureStatus.Running:
                    border?.SetBinding(Border.TranslationYProperty, new Binding("TraslationY", source: this));
                    int previous = TraslationY + (int)e.EventArgs.TotalY;
                    if (previous < 0)
                    {
                        if (e.EventArgs.TotalY < 0) 
                        { 
                            previous = 0;
                            Opened = true;
                        }
                    }
                    else Opened = false;
                    TraslationY = previous;
                    break;
                case GestureStatus.Canceled:
                case GestureStatus.Completed:
                    int finalY = TraslationY <= 500 ? 0 : 650;

#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    await border.TranslateTo(border.TranslationX, finalY, 250, Easing.Linear);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.
                    if (finalY == 0) Opened = true;
                    else Opened = false;
                    TraslationY = finalY;
                    break;
            } // End Switch
        } // End PanUpdate
    } // End LocationViewModel
}
