using Microsoft.Maui.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WeatherWiz.Models
{
    class TimeZoneService : Api
    {
        // Method
        public TimeZoneService() : base("https://api.geotimezone.com/public/")
        {
            HttpClient.BaseAddress = new Uri(URL ?? "");
        }
        /// <summary>
        /// Convert JSON to GeoTimeZoneResponse
        /// </summary>
        /// <param name="json">JSON to convert in GeoTimeZoneResponse</param>
        /// <returns>GeoTimeZoneResponse Object</returns>
        public static GeoTimeZoneResponse? ConvertJsonToGeoTimeZoneResponse(string json)
        {
            try
            {
                GeoTimeZoneResponse? response = JsonConvert.DeserializeObject<GeoTimeZoneResponse>(json);

                return response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Errore nella deserializzazione del JSON: " + ex.Message);
                return null;
            }
        } // End ConvertJsonToGeoTimeZoneResponse
        /// <summary>
        /// Get TimeZone based on Lat and Lon
        /// </summary>
        /// <param name="lat">Latitude of the place</param>
        /// <param name="lon">Longitude of the place</param>
        /// <returns>Timezone</returns>
        public async Task<string?> GetTimeZone(double? lat, double? lon)
        {
            try
            {
                var resp = await HttpClient.GetStringAsync($"timezone?latitude={lat?.ToString().Replace(",", ".")}&longitude={lon?.ToString().Replace(",", ".")}");
                return ConvertJsonToGeoTimeZoneResponse(resp)?.Offset;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: "+ex.Message);
                return "";
            }
        } // End GetTimeZone
        /// <summary>
        /// Get Time based on Lat and Lon
        /// </summary>
        /// <param name="lat">Latitude of the place</param>
        /// <param name="lon">Longitude of the place</param>
        /// <returns>TimeOnly</returns>
        public async Task<TimeOnly?> GetTimeOnly(double? lat, double? lon)
        {
            try
            {
                var resp = await HttpClient.GetStringAsync($"timezone?latitude={lat?.ToString().Replace(",", ".")}&longitude={lon?.ToString().Replace(",", ".")}");

                var data = ConvertJsonToGeoTimeZoneResponse(resp);

                return TimeOnly.FromDateTime(ConvertJsonToGeoTimeZoneResponse(resp).CurrentLocalDatetime);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: "+ex.Message);
                return null;
            }
        } // End GetTimeOnly
    } // End TimeZoneService
}
