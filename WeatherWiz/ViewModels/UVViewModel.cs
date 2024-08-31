using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherWiz.Models;

namespace WeatherWiz.ViewModels
{
	internal class UVViewModel : BaseViewModel
	{
		// Attribute
		private readonly UVService _uvService = new();
		private readonly Dictionary<int, string> scale = new () {
			{ 2, "Low" },
			{ 5, "Moderate" },
			{ 7, "High" },
			{ 10, "Very High" },
			{ int.MaxValue, "Extreme" },
		};

		private int _index;
		private string _description;
		private int _progress;
		private string? _timeSunRise;
		private string? _timeSunSet;

		// Property
		public int Index
		{
			get { return _index; }
			set 
			{ 
				if(SetProperty(ref _index, value))
				{
					Description = ScaleSelection(value);
					Progress = ProportionalProgress(value);
                } 
			}
		}
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }
        public int Progress
        {
            get { return _progress; }
            set { SetProperty(ref _progress, value); }
        }
        public string? TimeSunRise
        {
            get { return _timeSunRise; }
            set { SetProperty(ref _timeSunRise, value); }
        }
        public string? TimeSunSet
        {
            get { return _timeSunSet; }
            set { SetProperty(ref _timeSunSet, value); }
        }

        // Method
        public UVViewModel()
		{
			Index = -1;
			Task.Run(async () =>
			{
				var resp = await _uvService.GetCurrentUV(App.Coords.Item1.Value, App.Coords.Item2.Value);
				Index = (int)resp.Result.Uv;
                TimeSunRise = resp.Result.Sun_info.Sun_times.Sunrise.ToLocalTime().ToString("h:mm tt");
                TimeSunSet = resp.Result.Sun_info.Sun_times.Sunset.ToLocalTime().ToString("h:mmtt");
			});
		} // End Constructor
		private string ScaleSelection(int index)
		{
            foreach (var item in scale)
				if (index <= item.Key) return item.Value;
			return "";
        } // End ScaleSelection
		private int ProportionalProgress(int index)
		{
			return Math.Max(0, index * 14);
        } // End ProportionalProgress
    } // End UVViewModel
}
