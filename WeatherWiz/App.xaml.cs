using WeatherWiz.Util;
using WeatherWiz.ViewModels;

namespace WeatherWiz
{
    public partial class App : Application
    {
        public event Action<CurrentLocation>? CurrentLocationUpdated;
        private CurrentLocation? _currentLocation;

        public CurrentLocation? CurrentLocation
        {
            get { return _currentLocation; }
            set { _currentLocation = value; }
        }

        public App()
        {
            InitializeComponent();

            Task.Run(async () =>
            {
                CurrentLocation? CurrentLocation = await Helper.GetCurrentLocationAsync();

                CurrentLocationUpdated?.Invoke(CurrentLocation ?? new());
            });

            MainPage = new AppShell();
        }
    }
}
