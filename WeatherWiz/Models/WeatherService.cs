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
        public WeatherService() : base("https://api.openweathermap.org/data/2.5/") { }
        public async Task<WeatherFiveDaysResponse?> GetFiveDays(string CityName)
        {
            var response = await HttpClient.GetStringAsync($"forecast?q={CityName}&units=metric&appid=f61a63d6a6b9f3f16ed012d038973d46");

            var data = JsonConvert.DeserializeObject<WeatherFiveDaysResponse>(response);

            return data;
        } // End GetFiveDays
        public async Task<WeatherFiveDaysResponse?> GetToday(string CityName)
        {
            var response = await HttpClient.GetStringAsync($"forecast?q={CityName}&units=metric&appid=f61a63d6a6b9f3f16ed012d038973d46");

            var data = JsonConvert.DeserializeObject<WeatherFiveDaysResponse>(response);

            DateTime today = DateTime.Now.Date;

            data.List = data.List.Where(el => el.Dt_txt.Value.Date == today).ToList();

            return data;
        } // End GetToday
        public async Task<WeatherResumeResponse?> GetTodayResume(string CityName)
        {
            var response = await HttpClient.GetStringAsync($"weather?q={CityName}&units=metric&appid=f61a63d6a6b9f3f16ed012d038973d46");

            var data = JsonConvert.DeserializeObject<WeatherResumeResponse?>(response);

            return data;
        } // End GetTodayResume
    } // End WeatherService
}
