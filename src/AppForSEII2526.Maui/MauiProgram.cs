using Microsoft.Extensions.Localization;
using System.Globalization;
using Microsoft.Extensions.Logging;

namespace AppForSEII2526.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            // Recupera el idioma guardado o usa inglés por defecto
            var savedCulture = SecureStorage.Default.GetAsync("AppCulture").Result ?? "en";
            var culture = new CultureInfo(savedCulture);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            // Habilitar localización
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources/Languages");

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            return app;
        }
    }
}
