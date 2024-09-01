﻿using System;
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
        private WeatherFiveDaysResponse? _weatherWeekHourly;
        private List<WeatherDailySummary>? _weatherWeek;
        private int _temp;
        private string? _descr;
        private int _highTemp;
        private int _lowTemp;
        private int _translationY;
        private bool _opened;
        private double _windspeed;
        private double _winddeg;
        private double _rainfall;
        private double _tomorrowRain;

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
                        WeatherDay = await weatherService.GetTodayResume(value?.Split(",")[0] ?? "");
                        WeatherWeekHourly = await weatherService.GetFiveDays(value?.Split(",")[0] ?? "");
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
                    DateTime? dt_txt = DateTimeOffset.FromUnixTimeSeconds(value.dt).DateTime;
                    Forecasts.Add(new WeatherForecast() { 
                        Time = (DateTime)dt_txt, 
                        TimeDisplay = dt_txt.HasValue ? dt_txt.Value.ToString("H tt") : "", 
                        Image = $"https://openweathermap.org/img/wn/{value?.Weather?[0].Icon}@2x.png", 
                        Temperature = (int)value.Main.Temp });
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                    Description = textInfo.ToTitleCase(value?.Weather?[0].Description ?? "");
                    WindSpeed = value?.Wind == null ? 0 : value.Wind.Speed;
                    WindDeg = value?.Wind == null ? 0 : value.Wind.Deg;

                    Rainfall = value?.Rain == null ? 0d : value.Rain.Hour ?? 0d;
                } 
            }
        }
        public WeatherFiveDaysResponse? WeatherWeekHourly
        {
            get { return _weatherWeekHourly; }
            set 
            {
                if (SetProperty(ref _weatherWeekHourly, value))
                {
                    PopulateForecasts(value ?? new());
                    WeatherWeek = SummarizeByDay(value ?? new());
                } 
            }
        }
        public List<WeatherDailySummary>? WeatherWeek
        {
            get { return _weatherWeek; }
            set 
            { 
                if (SetProperty(ref _weatherWeek, value)) 
                {
                    TomorrowRain = Double.Round(value?[1].AvgRain ?? 0, 2);
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
        public double WindSpeed
        {
            get { return _windspeed; }
            set { SetProperty(ref _windspeed, value); }
        }
        public double WindDeg
        {
            get { return _winddeg; }
            set { SetProperty(ref _winddeg, value); }
        }
        public double Rainfall
        {
            get { return _rainfall; }
            set { SetProperty(ref _rainfall, value); }
        }
        public double TomorrowRain
        {
            get { return _tomorrowRain; }
            set { SetProperty(ref _tomorrowRain, value); }
        }

        // Method
        public MainPageViewModel()
        {
            Task.Run(async () => 
            {
                await UpdateTimeAsync();
            });

            _webView = new();

            _timer = new( 60 * 60 * 1000); 
            _timer.Elapsed += async (sender, e) => await UpdateTimeAsync();
            _timer.Start();

            TraslationY = 650;

            var app = (App)Application.Current;
            app.CurrentLocationUpdated += App_CurrentLocationUpdated;
        } // End Constructor
        private async void App_CurrentLocationUpdated(CurrentLocation obj)
        {
            Time = await tzService.GetTimeOnly(obj.Coords?.Item1, obj.Coords?.Item2);
            Location = obj.LocationResult;
        } // End App_LocationUpdated
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
        private void PopulateForecasts(WeatherFiveDaysResponse value)
        {
            DateTime? previousDate = null;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            value.List.ForEach(el =>
            {
#pragma warning disable CS8629 // Nullable value type may be null.
                DateTime currentDate = el.Dt_txt.Value.Date;
#pragma warning restore CS8629 // Nullable value type may be null.

                if (previousDate.HasValue && previousDate.Value != currentDate) Forecasts.Add(new Separator());

                Forecasts.Add(new WeatherForecast()
                {
                    Time = (DateTime)el.Dt_txt,
                    TimeDisplay = el.Dt_txt.HasValue ? el.Dt_txt.Value.ToString("H tt") : "",
                    Image = $"https://openweathermap.org/img/wn/{el.Weather?[0].Icon}@2x.png",
                    Temperature = (int)el.Main.Temp
                });
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                previousDate = currentDate;
            });

            DateTime currentTime = DateTime.Now;
            var weatherForecasts = Forecasts.OfType<WeatherForecast>();

            var closestForecast = weatherForecasts
                                    .OrderBy(f => Math.Abs((f.Time - currentTime).Ticks))
                                    .FirstOrDefault();

            if (closestForecast != null) closestForecast.TimeDisplay = "Now";
        } // End PopulateForecasts
        public List<WeatherDailySummary> SummarizeByDay(WeatherFiveDaysResponse value)
        {
            var dailyData = new Dictionary<string, List<WeatherDayResponse>>();

            foreach (var entry in value?.List ?? new())
            {
                string date = entry.Dt_txt.HasValue ? entry.Dt_txt.Value.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");

                if (!dailyData.ContainsKey(date))
                {
                    dailyData[date] = new List<WeatherDayResponse>();
                }

                dailyData[date].Add(entry);
            }

            // Calcola la media per ogni giorno
            var summarizedData = new List<WeatherDailySummary>();

            foreach (var day in dailyData)
            {
                var entries = day.Value;
                summarizedData.Add(new WeatherDailySummary
                {
                    AvgTemp = entries.Average(e => e.Main?.Temp ?? 0),
                    AvgFeelsLike = entries.Average(e => e.Main?.Feels_like ?? 0),
                    AvgHumidity = entries.Average(e => e.Main?.Humidity ?? 0),
                    AvgPressure = entries.Average(e => e.Main?.Pressure ?? 0),
                    AvgRain = entries.Average(e => e.Rain?.ThreeHours ?? 0),
                    WeatherDescription = entries.GroupBy(e => e.Weather?[0].Description)
                                                .OrderByDescending(g => g.Count())
                                                .First().Key,
                    Date = DateTime.Parse(day.Key)
                });
            }

            return summarizedData;
        }
    } // End LocationViewModel
}
