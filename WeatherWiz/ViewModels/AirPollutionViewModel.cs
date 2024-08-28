using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherWiz.Models;
using WeatherWiz.Util;

namespace WeatherWiz.ViewModels
{
    public class AirQualityRow
    {
		// Property
        public int AQI { get; set; }
        public float Concentration { get; set; }
        public string Category { get; set; }

		// Method
        public AirQualityRow(int aqi, float concentration, string category)
        {
            AQI = aqi;
            Concentration = concentration;
            Category = category;
        }
    } // End AirQualityRow
    public class AirPollutionViewModel : BaseViewModel
	{
		// Attribute
		private readonly Dictionary<int, string> _dict = new()
		{
			{50, "1-Good"},
			{100, "2-Moderate" },
			{150, "3-Unhealthy for Sensitive Groups" },
			{200, "4-Unhealthy" },
			{300, "5-Very Unhealthy" },
			{int.MaxValue, "6-Hazardous" }
		};
        private readonly Dictionary<string, (double[] concentrations, int[] aqiValues)> breakpoints = new()
        {
            // PM2.5 (in µg/m³)
            { "pm2_5", (new double[] { 0.0, 12.0, 35.4, 55.4, 150.4, 250.4, 500.0 },
                        new int[] { 0, 50, 100, 150, 200, 300, 500 }) },

            // PM10 (in µg/m³)
            { "pm10", (new double[] { 0.0, 54.0, 154.0, 254.0, 354.0, 424.0, 604.0 },
                       new int[] { 0, 50, 100, 150, 200, 300, 500 }) },

            // O3 (in µg/m³)
            { "o3", (new double[] { 0.0, 160, 200, 300, 400, 800 },
                        new int[] { 0, 50, 100, 150, 200, 300 }) },

            // CO (in µg/m³)
            { "co", (new double[] { 0.0, 515.0, 1090.0, 1500.0, 1800.0, 3470.0, 5100.0 },
                        new int[] { 0, 50, 100, 150, 200, 300, 500 }) },

            // SO2 (in µg/m³)
            { "so2", (new double[] { 0.0, 1.42, 3.18, 7.55, 12.90, 25.93, 43.42 },
                        new int[] { 0, 50, 100, 150, 200, 300, 500 }) },

            // NO2 (in µg/m³)
            { "no2", (new double[] { 0.0, 2.17, 4.64, 14.82, 27.91, 57.83, 94.60 },
                        new int[] { 0, 50, 100, 150, 200, 300, 500 }) }
        };
        private readonly WeatherService weatherService = new();

        private WeatherAirPollutionResponse? _aqiState;
		private Tuple<double?, double?>? _coords;
        private int _aqi;
        private string? _description;
		private float _progress;

		// Property
		public WeatherAirPollutionResponse? AQIState
		{
			get { return _aqiState; }
			set 
			{ 
				if (SetProperty(ref _aqiState, value))
				{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    AQI = CalculateOverallAQI(value.List[0].Components.GetType()
                            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                            .ToDictionary(prop => prop.Name, prop => (double)prop.GetValue(value.List[0].Components, null))
                        ); 
#pragma warning restore CS8602 // Dereference of a possibly null reference.
				}
			}
		}
		public Tuple<double?, double?>? Coords
		{
			get { return _coords; }
			set 
			{ 
				if (SetProperty(ref _coords, value)) 
				{
                    Task.Run(async () =>
                    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8629 // Nullable value type may be null.
                        AQIState = await weatherService.GetAirPollution(value.Item1.Value, value.Item2.Value);
#pragma warning restore CS8629 // Nullable value type may be null.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    });
                } 
			}
		}
        public string? Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }
        public float Progress
        {
            get { return _progress; }
            set { SetProperty(ref _progress, value); }
        }
        public int AQI
        {
            get { return _aqi; }
            set 
            { 
                if(SetProperty(ref _aqi, value))
                {
                    Description = ScalePollution(value);
                    Progress = value;
                } 
            }
        }


        // Method
        public AirPollutionViewModel() 
		{
            Task.Run(async () =>
            {
                var resp = await Helper.GetCurrentLocationAsync();
				Coords = resp?.Coords;
            });

        } // End Constructor
        public int CalculateOverallAQI(Dictionary<string, double> components)
        {
            var aqiValues = new List<int>();

            foreach (var component in components)
            {
                if (breakpoints.ContainsKey(component.Key.ToLower()))
                {
                    var (concentrations, aqiValuesArray) = breakpoints[component.Key.ToLower()];
                    aqiValues.Add(CalculateAQI(component.Value, concentrations, aqiValuesArray));
                }
            }
            return aqiValues.Max();
        } // End CalculateOverallAQI
        private int CalculateAQI(double concentration, double[] concentrations, int[] aqiValues)
        {
            for (int i = 0; i < concentrations.Length - 1; i++)
            {
                if (concentration >= concentrations[i] && concentration <= concentrations[i + 1])
                {
                    return (int)((aqiValues[i + 1] - aqiValues[i]) /
                                 (concentrations[i + 1] - concentrations[i]) *
                                 (concentration - concentrations[i]) + aqiValues[i]);
                }
            }
            return -1;
        } // End CalculateAQI
        private string ScalePollution(int AQI)
		{
            foreach (var item in _dict)
				if (AQI <= item.Key) return item.Value;
			return string.Empty;
        } // End ScalePollution
        public async Task HrefCommand(string url)
        {
            await Launcher.OpenAsync(url);
        } // End HrefCommand

    } // End AirPollutionViewModel
}
