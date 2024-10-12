using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWiz.Models;
using WeatherWiz.Util;

namespace WeatherWiz.ViewModels
{
    public class TimeViewModel : BaseViewModel
    {
        private readonly TimeZoneService tzService = new();
        private readonly System.Timers.Timer _timer;
        private TimeOnly? _time;
        private ImageSource? _imageSource;
        private readonly WebView? _webView;

        public TimeOnly? Time
        {
            get => _time;
            set
            {
                if (SetProperty(ref _time, value))
                {
                    UpdateImageSource();
                }
            }
        }
        public ImageSource? ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        public TimeViewModel()
        {
            Task.Run(async () =>
            {
                await UpdateTimeAsync();
            });

            _webView = new();

            _timer = new System.Timers.Timer(60 * 60 * 1000);
            _timer.Elapsed += async (sender, e) => await UpdateTimeAsync();
            _timer.Start();

            var app = (App)App.Current;
            if (app == null) return;

            app.CurrentLocationUpdated += App_CurrentLocationUpdated;

        } // End Constructor
        private async void App_CurrentLocationUpdated(CurrentLocation obj)
        {
            Time = await tzService.GetTimeOnly(obj.Coords?.Item1, obj.Coords?.Item2);
        } // End App_CurrentLocationUpdated
        private async Task UpdateTimeAsync()
        {
            if (Time == null || _webView == null) return;
            await _webView.EvaluateJavaScriptAsync($"updateTime('{Time.Value:HH:mm}')");
        }
        private void UpdateImageSource()
        {
            if (Time == null) return;
            if (Time.Value.Hour is >= 6 and < 8) ImageSource = "sunrise.jpg";
            else if (Time.Value.Hour is >= 8 and < 18) ImageSource = "day.jpg";
            else if (Time.Value.Hour is >= 18 and < 19) ImageSource = "sunset.jpg";
            else ImageSource = "night.jpg";
        } // End UpdateImageSource
    } // End TimeViewModel
}
