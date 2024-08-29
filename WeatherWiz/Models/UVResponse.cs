using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWiz.Models
{
    public class UVResponse : ObjectLog
    {
        public UVResultResponse? Result { get; set; }
    }
    public class UVResultResponse
    {
        public double Uv { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Uv_time { get; set; }
        public double Uv_max { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Uv_max_time { get; set; }
        public double Ozone { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Ozone_time { get; set; }
        public UVSafeExposureTimeResponse? Safe_exposure_time { get; set; }
        public UVSunInfoResponse? Sun_info { get; set; }
    } // End UVResultResponse
    public class UVSafeExposureTimeResponse
    {
        public object? St1 { get; set; }
        public object? St2 { get; set; }
        public object? St3 { get; set; }
        public object? St4 { get; set; }
        public object? St5 { get; set; }
        public object? St6 { get; set; }
    } // End UVSafeExposureTimeResponse
    public class UVSunInfoResponse
    {
        public UVSunTimesResponse? Sun_times { get; set; }
        public UVSunPositionResponse? Sun_position { get; set; }
    } // End UVSunInfoResponse
    public class UVSunTimesResponse
    {
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Solarnoon { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Nadir { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Sunrise { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Sunset { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Sunriseend { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Sunsetstart { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Dawn { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Dusk { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Nauticaldawn { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Nauticaldusk { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Nightend { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Night { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Goldenhourend { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Goldenhour { get; set; }
    } // End UVSunTimesResponse
    public class UVSunPositionResponse
    {
        public double Azimuth { get; set; }
        public double Altitude { get; set; }
    }
}
