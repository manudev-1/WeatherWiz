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
        public UVService() : base("https://api.openuv.io/api/v1/", Environment.GetEnvironmentVariable("ApiKeyOpenUV") ?? "") { }
        public async Task<UVResponse?> GetCurrentUV(double lat, double lon)
        {
            HttpClient.DefaultRequestHeaders.Add("x-access-token", ApiKey);

            var response = await HttpClient.GetStringAsync($"uv?lat={lat}&lng={lon}");
            var data = JsonConvert.DeserializeObject<UVResponse?>(response);

            return data;
        }
    } // End UVService
}
