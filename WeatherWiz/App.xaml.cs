using WeatherWiz.Util;

namespace WeatherWiz
{
    public partial class App : Application
    {
        public static string? CityName { get; set; }
        public static Tuple<double?, double?>? Coords { get; set; }
        public App()
        {
            InitializeComponent();

            Task.Run(async () =>
            {
                CurrentLocation? resp = await Helper.GetCurrentLocationAsync();
                Coords = resp?.Coords;
                CityName = resp?.LocationResult;
            });

            MainPage = new AppShell();
        }
    }
}
