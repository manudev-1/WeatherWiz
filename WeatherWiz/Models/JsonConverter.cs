using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Diagnostics;

namespace WeatherWiz.Models
{
    public class CustomDateTimeConverter : DateTimeConverterBase
    {
        private const string FormatWithMilliseconds = "yyyy-MM-ddTHH:mm:ss.fff";
        private const string FormatWithMillisecondsAndZ = "yyyy-MM-ddTHH:mm:ss.fffZ";
        private const string FormatDayMonthYear = "dd/MM/yyyy HH:mm:ss";
        private const string FormatYearMonthDay = "yyyy-MM-dd HH:mm:ss";

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            string dateString = (reader.Value?.ToString()) ?? throw new JsonSerializationException("Date string is null.");

            if (DateTime.TryParseExact(dateString, FormatWithMillisecondsAndZ, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime dateTimeWithZ)) return dateTimeWithZ;
            if (DateTime.TryParseExact(dateString, FormatWithMilliseconds, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime)) return dateTime;
            if (DateTime.TryParseExact(dateString, FormatDayMonthYear, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTimeDayMonthYear)) return dateTimeDayMonthYear;
            if (DateTime.TryParseExact(dateString, FormatYearMonthDay, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTimeYearMonthDay)) return dateTimeYearMonthDay;
            

            throw new JsonSerializationException($"Impossibile convertire il valore '{dateString}' in DateTime.");
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            DateTime? valueDT = value as DateTime?;

            if (valueDT?.Kind == DateTimeKind.Utc)
                writer.WriteValue(valueDT?.ToString(FormatWithMillisecondsAndZ, CultureInfo.InvariantCulture));
            else
                writer.WriteValue(valueDT?.ToString(FormatWithMilliseconds, CultureInfo.InvariantCulture));
        }
    }
}
