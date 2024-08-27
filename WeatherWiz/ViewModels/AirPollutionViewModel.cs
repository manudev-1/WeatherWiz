using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWiz.ViewModels
{
    internal class AirPollutionViewModel : BaseViewModel
    {
		private int _test;

		public int Test
		{
			get { return _test; }
			set { SetProperty(ref _test, value); }
		}

		public AirPollutionViewModel() 
		{
			Test = 1;
		}

	}
}
