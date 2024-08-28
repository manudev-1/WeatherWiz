using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWiz.Models
{
    public class WeatherFiveDaysResponse : ObjectLog
    {
        public string? Cod { get; set; }
        public int Message { get; set; }
        public int Cnt { get; set; }
        public List<WeatherDayResponse>? List { get; set; }
    } // End WeatherFiveDaysResponse
    public class WeatherDayResponse
    {
        public int Dt { get; set; }
        public WeatherDayMainResponse? Main { get; set; }
        public List<WeatherDaySpecResponse>? Weather { get; set; }
        public WeatherDayCloudResponse? Clouds { get; set; }
        public WeatherDayWindResponse? Wind { get; set; }
        public int Visibility { get; set; }
        public double Pop { get; set; }
        public WeatherDaySysResponse? Sys { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? Dt_txt { get; set; }
    } // End WeatherFiveDaysResponse
    public class WeatherDayMainResponse
    {
        public double Temp { get; set; }
        public double Feels_like { get; set; }
        public double Temp_min { get; set; }
        public double Temp_max { get; set; }
        public int Pressure { get; set; }
        public int Sea_level { get; set; }
        public int Grnd_level { get; set; }
        public int Humidity { get; set; }
        public double Temp_kf { get; set; }
    } // End WeatherDayMainResponse
    public class WeatherDaySpecResponse
    {
        public int Id { get; set; }
        public string? Main { get; set; }
        public string? Description { get; set; }
        public string Icon { get; set; }
    } // End WeatherDaySpecResponse
    public class WeatherDayCloudResponse
    {
        public int All { get; set; }
    } // End WeatherDayCloudResponse
    public class WeatherDayWindResponse
    {
        public double Speed { get; set; }
        public int Deg { get; set; }
        public double gust { get; set; }
    } // End WeatherDayWindResponse
    public class WeatherDaySysResponse
    {
        public string? Pod { get; set; }
    } // End WeatherDaySysResponse

    public class WeatherResumeResponse : ObjectLog
    {
        public WeatherResumeCoordResponse? Coord { get; set; }
        public List<WeatherResumeWeatherResponse>? Weather { get; set; }
        public string? Base { get; set; }
        public WeatherResumeMainResponse? Main { get; set; }
        public int Visibility { get; set; }
        public WeatherResumeWindResponse? Wind { get; set; }
        public WeatherDayCloudResponse? Cloud { get; set; }
        public int dt { get; set; }
        public WeatherResumeSysResponse? Sys { get; set; }
        public int Timezone { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Cod { get; set; }
    } // End WeatherResumeResponse
    public class WeatherResumeCoordResponse
    {
        public double Lon { get; set; }
        public double Lat { get; set; }
    } // End WeatherResumeCoordResponse
    public class WeatherResumeWeatherResponse
    {
        public int Id { get; set; }
        public string? Main { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
    } // End WeatherResumeWeatherResponse
    public class WeatherResumeMainResponse
    {
        public double Temp { get; set; }
        public double Feels_like { get; set; }
        public double Temp_min { get; set; }
        public double Temp_max { get; set; }
        public int Pressure { get; set; }
        public int Sea_level { get; set; }
        public int Grnd_level { get; set; }
        public int Humidity { get; set; }
    } // End WeatherResumeMainResponse
    public class WeatherResumeWindResponse
    {
        public double Speed { get; set; }
        public int Deg { get; set; }
    } // End WeatherResumeWindResponse
    public class WeatherResumeSysResponse
    {
        public int Type { get; set; }
        public int Id { get; set; }
        public string? Country { get; set; }
        public int Sunrise { get; set; }
        public int Sunset { get; set; }
    } // End WeatherResumeSysResponse

    public class WeatherAirPollutionResponse : ObjectLog
    {
        public WeatherResumeCoordResponse? Coord { get; set; }
        public List<WeatherAirDataResponse>? List { get; set; }

    } // End WeatherAirPollutionResponse
    public class WeatherAirDataResponse
    {
        public WeatherAirDataMainResponse? Main { get; set; }
        public WeatherAirDataCompResponse? Components { get; set; }
        public int Dt { get; set; }
    } // End WeatherAirDataResponse
    public class WeatherAirDataMainResponse
    {
        public int Aqi { get; set; }
    } // End WeatherAirDataMainResponse
    public class WeatherAirDataCompResponse
    {
        public double Co { get; set; }
        public double No { get; set; }
        public double No2 { get; set; }
        public double O3 { get; set; }
        public double So3 { get; set; }
        public double Pm2_5 { get; set; }
        public double Pm10 { get; set; }
        public double Nh3 { get; set; }
    } // End WeatherAirDataCompResponse
} // End Namespace