using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWiz.Models
{
    public class WeatherForecast
    {
        public DateTime Time { get; set; }
        public string? TimeDisplay { get; set; } = string.Empty;
        public ImageSource? Image { get; set; }
        public int Temperature { get; set; }
        public Color? TimeLabelColor
        {
            get
            {
                if (TimeDisplay == "Now")
                    return Color.FromArgb("48319D");
                else return Color.FromArgb("00000000");
            }
        }
    }
}
