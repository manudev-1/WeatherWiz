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
        public string? Time { get; set; } = string.Empty;
        public ImageSource? Image { get; set; }
        public int Temperature { get; set; }
        public Color? TimeLabelColor
        {
            get
            {
                if (Time == "Now")
                    return Color.FromArgb("48319D");
                else return Color.FromArgb("00000000");
            }
        }
    }
}
