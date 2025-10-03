using CommunityToolkit.Maui;
using MauiFrontend.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MauiFrontend.Services;
using MauiFrontend.ViewModels;
using MauiFrontend.Views;

namespace MauiFrontend
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Poppins-Regular.ttf", "PoppinsRegular");
                    fonts.AddFont("Poppins-Thin.ttf", "PoppinsThin");
                    fonts.AddFont("Poppins-Bold.ttf", "PoppinsBold");
                    fonts.AddFont("Poppins-ExtraBold.ttf", "PoppinsExtraBold");
                });

            builder.AddAppSettings();

            string baseApi = builder.Configuration.GetValue<string>("ApiBaseUrl");

            baseApi ??= "localhost:8080";

            builder
                .Services
                .AddHttpClient<Https>(x =>
                {
                    x.BaseAddress = new Uri(baseApi);
                });

            // Dependency injection
            // Services
            builder.Services.AddTransient<UserService>();
            builder.Services.AddTransient<ProductService>();

            // ViewModels
            builder.Services.AddTransient<LoginViewModel>();

            // Page
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<MainPage>();



#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static void AddAppSettings(this MauiAppBuilder builder)
        {
            using Stream stream = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream("MauiFrontend.appsettings.json");

            if (stream != null)
            {
                IConfigurationRoot config = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();

                builder.Configuration.AddConfiguration(config);
            }
        }
    }
}
