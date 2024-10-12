using Microsoft.Maui;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WeatherWiz.Models;
using WeatherWiz.Util;

namespace WeatherWiz.ViewModels
{
    public class WeatherViewModel : BaseViewModel
    {
        private readonly WeatherService weatherService = new();
        private WeatherFiveDaysResponse? _weatherWeekHourly;
        private WeatherResumeResponse? _weatherDay;
        private ObservableCollection<WeatherDailySummary>? _weatherWeek;
        private int _temp;
        private int _highTemp;
        private int _lowTemp;
        private string? _descr;
        private double _windspeed;
        private double _winddeg;
        private double _rainfall;
        private double _tomorrowRain;
        private int _feelsLike;
        private string? _feelPunchLine;
        private int _humidity;
        private int _dewPoint;
        private int _visibility;
        private string? _visibilityLine;
        private int _pressure;
        private string? _pressureLine;

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
        public WeatherResumeResponse? WeatherDay
        {
            get => _weatherDay;
            set
            {
                if (SetProperty(ref _weatherDay, value))
                {
                    UpdateWeatherData(value);
                }
            }
        }
        public ObservableCollection<WeatherDailySummary>? WeatherWeek
        {
            get => _weatherWeek;
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
        public ObservableCollection<object> Forecasts { get; set; } = new();
        public string? Description
        {
            get { return _descr; }
            set { SetProperty(ref _descr, value); }
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
        public int FeelsLike
        {
            get { return _feelsLike; }
            set
            {
                if (SetProperty(ref _feelsLike, value))
                {
                    FeelPunchLine = GetPerceivedTemperatureDescription(value, Temp);
                }
            }
        }
        public string? FeelPunchLine
        {
            get { return _feelPunchLine; }
            set { SetProperty(ref _feelPunchLine, value); }
        }
        public int Humidity
        {
            get { return _humidity; }
            set
            {
                if (SetProperty(ref _humidity, value))
                {
                    DewPoint = (int)CalculateDewPoint(Temp, value);
                }
            }
        }
        public int DewPoint
        {
            get { return _dewPoint; }
            set { SetProperty(ref _dewPoint, value); }
        }
        public int Visibility
        {
            get { return _visibility; }
            set
            {
                if (SetProperty(ref _visibility, value))
                {
                    VisibilityLine = GetVisibilityDescription(value);
                }
            }
        }
        public string? VisibilityLine
        {
            get { return _visibilityLine; }
            set { SetProperty(ref _visibilityLine, value); }
        }
        public int Pressure
        {
            get { return _pressure; }
            set
            {
                if (SetProperty(ref _pressure, value))
                {
                    PressureLine = GetPressureDescription(value);
                }
            }
        }
        public string? PressureLine
        {
            get { return _pressureLine; }
            set { SetProperty(ref _pressureLine, value); }
        }

        public WeatherViewModel()
        {
            var app = (App)Application.Current;
            if (app == null) return;

            app.CurrentLocationUpdated += App_CurrentLocationUpdated;
        } // End Constructor
        private async void App_CurrentLocationUpdated(CurrentLocation obj)
        {
            WeatherDay = await weatherService.GetTodayResume(obj.LocationResult?.Split(",")[0] ?? "");
            WeatherWeekHourly = await weatherService.GetFiveDays(obj.LocationResult?.Split(",")[0] ?? "");
        } // End App_LocationUpdated
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
        public ObservableCollection<WeatherDailySummary> SummarizeByDay(WeatherFiveDaysResponse value)
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
                    AvgTemp = (int)entries.Average(e => e.Main?.Temp ?? 0),
                    AvgFeelsLike = entries.Average(e => e.Main?.Feels_like ?? 0),
                    AvgHumidity = entries.Average(e => e.Main?.Humidity ?? 0),
                    AvgPressure = entries.Average(e => e.Main?.Pressure ?? 0),
                    AvgRain = entries.Average(e => e.Rain?.ThreeHours ?? 0),
                    WeatherDescription = entries.GroupBy(e => e.Weather?[0].Description)
                                                .OrderByDescending(g => g.Count())
                                                .First().Key,
                    Icon = $"https://openweathermap.org/img/wn/{entries.GroupBy(e => e.Weather?[0].Icon)
                                                                       .OrderByDescending(g => g.Count())
                                                                       .First().Key}@2x.png",
                    Date = DateTime.Parse(day.Key)
                });
            }

            return new(summarizedData);
        } // End SummarizeByDay
        private void UpdateWeatherData(WeatherResumeResponse? weatherData)
        {
            if (weatherData != null)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                Temp = (int)weatherData.Main.Temp;
                HigherTemp = (int)weatherData.Main.Temp_max;
                LowerTemp = (int)weatherData.Main.Temp_min;
                DateTime? dt_txt = DateTimeOffset.FromUnixTimeSeconds(weatherData.dt).DateTime;
                Forecasts.Add(new WeatherForecast()
                {
                    Time = (DateTime)dt_txt,
                    TimeDisplay = dt_txt.HasValue ? dt_txt.Value.ToString("H tt") : "",
                    Image = $"https://openweathermap.org/img/wn/{weatherData?.Weather?[0].Icon}@2x.png",
                    Temperature = (int)weatherData.Main.Temp
                });
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                Description = textInfo.ToTitleCase(weatherData?.Weather?[0].Description ?? "");
                WindSpeed = weatherData?.Wind == null ? 0 : weatherData.Wind.Speed;
                WindDeg = weatherData?.Wind == null ? 0 : weatherData.Wind.Deg;
                Rainfall = weatherData?.Rain == null ? 0d : weatherData.Rain.Hour ?? 0d;
                FeelsLike = weatherData == null ? 0 : (int)weatherData.Main.Feels_like;
                Humidity = weatherData == null ? 0 : weatherData.Main.Humidity;
                Visibility = MeterToKilometer(weatherData?.Visibility ?? 0);
                Pressure = weatherData == null ? 0 : weatherData.Main.Pressure;
            }
        } // End UpdateWeatherData
        private int MeterToKilometer(int value)
        {
            return value / 1000;
        } // End MeterToKilometer
        private string GetPerceivedTemperatureDescription(double perceivedTemperature, double actualTemperature)
        {
            double difference = perceivedTemperature - actualTemperature;

            if (Math.Abs(difference) < 1) return "Temperature is almost the same.";
            else if (difference > 0 && difference <= 3) return "Slightly warmer than expected.";
            else if (difference > 3) return "Significantly warmer than expected.";
            else if (difference < 0 && difference >= -3) return "Slightly cooler than expected.";
            return "Significantly cooler than expected.";
        } // End GetPerceivedTemperatureDescription
        private double CalculateDewPoint(double temperature, double humidity)
        {
            double a = 17.27;
            double b = 237.7;

            double alpha = ((a * temperature) / (b + temperature)) + Math.Log(humidity / 100.0);
            double dewPoint = (b * alpha) / (a - alpha);

            return dewPoint;
        } // End CalculateDewPoint
        private string GetVisibilityDescription(double visibilityInKm)
        {
            if (visibilityInKm >= 10) return "Clear visibility.";
            else if (visibilityInKm >= 5 && visibilityInKm < 10) return "Good visibility.";
            else if (visibilityInKm >= 1 && visibilityInKm < 5) return "Moderate visibility.";
            else if (visibilityInKm >= 0.5 && visibilityInKm < 1) return "Poor visibility.";
            return "Very poor visibility.";
        } // End GetVisibilityDescription
        private string GetPressureDescription(double pressureInHpa)
        {
            if (pressureInHpa > 1020)
                return "High pressure, clear skies.";
            else if (pressureInHpa >= 1000 && pressureInHpa <= 1020)
                return "Stable pressure, typical conditions.";
            else if (pressureInHpa < 1000)
                return "Low pressure, possible storms.";
            return "";
        } // End GetPressureDescription
    } // End WeatherViewModel

}
