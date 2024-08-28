using Newtonsoft.Json;
using ObjCRuntime;
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
        public double UV { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime UVTime { get; set; }
        public double UVMax { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime UVMaxTime { get; set; }
        public double Ozone { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime OzoneTime { get; set; }
        public UVSafeExposureTimeResponse? SafeExposureTime { get; set; }
        public UVSunInfoResponse? SunInfo { get; set; }
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
        public UVSunTimesResponse? SunTimes { get; set; }
        public UVSunPositionResponse? SunPosition { get; set; }
    } // End UVSunInfoResponse
    public class UVSunTimesResponse
    {
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime SolarNoon { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Nadir { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Sunrise { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Sunset { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime SunriseEnd { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime SunsetStart { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Dawn { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Dusk { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime NauticalDawn { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime NauticalDusk { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime NightEnd { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Night { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime GoldenHourEnd { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime GoldenHour { get; set; }
    } // End UVSunTimesResponse
    public class UVSunPositionResponse
    {
        public double Azimuth { get; set; }
        public double Altitude { get; set; }
    }
}
