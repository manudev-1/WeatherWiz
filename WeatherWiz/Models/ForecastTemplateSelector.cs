using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWiz.Models
{
    class ForecastTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? ForecastTemplate { get; set; }
        public DataTemplate? SeparatorTemplate { get; set; }
        public DataTemplate? WeekForecastTemplate { get; set; }
        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            if (item is Separator)
                return SeparatorTemplate ?? new();

            if (item is WeatherForecast)
                return ForecastTemplate ?? new();

            if (item is WeatherDailySummary)
                return WeekForecastTemplate ?? new();

            return null;
        }
    }
}
