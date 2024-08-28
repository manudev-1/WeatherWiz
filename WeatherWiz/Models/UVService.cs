using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWiz.Models
{
    internal class UVService : Api
    {
        public UVService() : base("https://api.openuv.io/api/v1/", Environment.GetEnvironmentVariable("ApiKeyOpenWeather") ?? "") { }
        public async Task<UVResponse?> GetCurrentUV(double lat, double lon)
        {
            this.HttpClient.DefaultRequestHeaders.Add("x-access-token", ApiKey);
            var response = await HttpClient.GetStringAsync($"weather?lat={lat}&lon={lon}");
            Debug.WriteLine(response);
            var data = JsonConvert.DeserializeObject<UVResponse?>(response);

            return data;
        }
    } // End UVService
}
