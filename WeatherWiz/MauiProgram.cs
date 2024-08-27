using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
using WeatherWiz.Models;

namespace WeatherWiz
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureLifecycleEvents(events =>
                {
#if ANDROID
                events.AddAndroid(android => android.OnCreate((activity, bundle) => MakeStatusBarTranslucent(activity)));

                static void MakeStatusBarTranslucent(Android.App.Activity activity)
                {
                    activity.Window?.SetFlags(Android.Views.WindowManagerFlags.LayoutNoLimits, Android.Views.WindowManagerFlags.LayoutNoLimits);

                    activity.Window?.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);

                    activity.Window?.SetStatusBarColor(Android.Graphics.Color.Transparent);
                }
#endif
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcon");
                    fonts.AddFont("Montserrat-Bold.ttf", "MontSerratBold");
                    fonts.AddFont("Montserrat-Medium.ttf", "MontSerratMedium");
                    fonts.AddFont("Montserrat-Regular.ttf", "MontSerrat");
                    fonts.AddFont("Montserrat-SemiBold.ttf", "MontSerratSemibold");
                    fonts.AddFont("Montserrat-Thin.ttf", "MontSerratThin");
                });

            var a = Assembly.GetExecutingAssembly();
            using Stream? stream = a.GetManifestResourceStream("WeatherWiz.appsettings.json");

#pragma warning disable CS8604 // Possible null reference argument.
            var config = new ConfigurationBuilder()
                        .AddJsonStream(stream)
                        .Build();
#pragma warning restore CS8604 // Possible null reference argument.

            foreach (var kv in config.AsEnumerable())
                Environment.SetEnvironmentVariable(kv.Key, kv.Value);
            
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
