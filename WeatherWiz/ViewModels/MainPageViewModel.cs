using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherWiz.Models;

namespace WeatherWiz.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        // Attribute
        private TimeZoneService tzService = new();
        private string? _location;
        private TimeOnly? _time; 
        private Tuple<double?, double?>? _coords;
        private ImageSource? _imageSource;

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


        // Method
        public MainPageViewModel()
        {
            Task.Run(async () => await GetCurrentLocationAsync());
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
    } // End LocationViewModel
}
