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
        public WeatherService() : base("https://api.openweathermap.org/data/2.5/", Environment.GetEnvironmentVariable("ApiKey") ?? "") { }
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

            data.List = data.List.Where(el => el.Dt_txt.Value.Date == today).ToList();

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
    } // End WeatherService
}
