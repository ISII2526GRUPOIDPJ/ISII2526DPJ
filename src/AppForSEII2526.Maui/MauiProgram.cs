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

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("nunito.ttf", "Nunito");
                });

            builder.Services.AddMauiBlazorWebView();

            // Configurar localización
            builder.Services.AddLocalization();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}