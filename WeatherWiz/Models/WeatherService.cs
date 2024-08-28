using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWiz.Models
{
    internal class WeatherService : Api
    {
        public WeatherService() : base("https://api.openweathermap.org/data/2.5/", Environment.GetEnvironmentVariable("ApiKeyOpenWeather") ?? "") { }
        /// <summary>
        /// Get next five days of weather
        /// </summary>
        /// <param name="CityName">Name of the City which you want weather</param>
        /// <returns>Weather of the City from today to five days</returns>
        public async Task<WeatherFiveDaysResponse?> GetFiveDays(string CityName)
        {
            var response = await HttpClient.GetStringAsync($"forecast?q={CityName}&units=metric&appid={ApiKey}");

            var data = JsonConvert.DeserializeObject<WeatherFiveDaysResponse>(response);

            return data;
        } // End GetFiveDays
        /// <summary>
        /// Get today weather
        /// </summary>
        /// <param name="CityName">Name of the City which you want weather</param>
        /// <returns>Weather of the City of today</returns>
        public async Task<WeatherFiveDaysResponse?> GetToday(string CityName)
        {
            var response = await HttpClient.GetStringAsync($"forecast?q={CityName}&units=metric&appid={ApiKey}");

            var data = JsonConvert.DeserializeObject<WeatherFiveDaysResponse>(response);

            DateTime today = DateTime.Now.Date;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8629 // Nullable value type may be null.
            data.List = data.List?.Where(el => el.Dt_txt.Value.Date == today).ToList();
#pragma warning restore CS8629 // Nullable value type may be null.
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            return data;
        } // End GetToday
        /// <summary>
        /// Get today resume of weather
        /// </summary>
        /// <param name="CityName">Name of the City which you want weather</param>
        /// <returns>Weather resume of the City of today</returns>
        public async Task<WeatherResumeResponse?> GetTodayResume(string CityName)
        {
            var response = await HttpClient.GetStringAsync($"weather?q={CityName}&units=metric&appid={ApiKey}");

            var data = JsonConvert.DeserializeObject<WeatherResumeResponse?>(response);

            return data;
        } // End GetTodayResume
        /// <summary>
        /// AirPollution by Lat and Lon
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="lon">Longitude</param>
        /// <returns>Return air pollution data about the area</returns>
        public async Task<WeatherAirPollutionResponse?> GetAirPollution(double lat, double lon)
        {
            var response = await HttpClient.GetStringAsync($"air_pollution?lat={lat}&lon={lon}&appid={ApiKey}");
            Debug.WriteLine("CAZZO: " + lat + " " + lon);
            var data = JsonConvert.DeserializeObject<WeatherAirPollutionResponse?>(response);

            return data;
        }
    } // End WeatherService
}
