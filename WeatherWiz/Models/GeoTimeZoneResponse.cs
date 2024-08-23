using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWiz.Models
{
    class GeoTimeZoneResponse
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string? Location { get; set; }
        public string? CountryIso { get; set; }
        public string? IanaTimezone { get; set; }
        public string? TimezoneAbbreviation { get; set; }
        public string? DstAbbreviation { get; set; }
        public string? Offset { get; set; }
        public string? DstOffset { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        [JsonProperty("current_local_datetime")]
        public DateTime CurrentLocalDatetime { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        [JsonProperty("current_utc_datetime")]
        public DateTime CurrentUtcDatetime { get; set; }

    }
}
