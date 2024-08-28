using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWiz.Util
{
    class CurrentLocation
    {
        public Tuple<double?, double?>? Coords { get; set; }
        public string? LocationResult { get; set; }
    }
    class Helper
    {
        /// <summary>
        /// Get The Current Location of the phone
        /// </summary>
        /// <returns>Current location based on GPS</returns>
        public static async Task<CurrentLocation?> GetCurrentLocationAsync()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                CurrentLocation result = new()
                {
                    Coords = (Tuple<double?, double?>?)null,
                    LocationResult = ""
                };

                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.High,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
                }

                result.Coords = Tuple.Create(location?.Latitude, location?.Longitude);

                if (location != null)
                {
                    var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                    var placemark = placemarks?.FirstOrDefault();

                    if (placemark != null)
                    {
                        result.LocationResult = $"{placemark.Locality}, {placemark.CountryName}";
                        return result;
                    } // End nested if
                    result.LocationResult = "Posizione non trovata";
                    return result;
                } // End if
                result.LocationResult = "Impossibile ottenere la posizione";
                return result;
            } // End try
            catch (Exception ex)
            {
                return null;
            } // End catch
        } // End GetCurrentLocationAsync
    } // End Helper Class
}
