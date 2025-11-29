// MainProgram.cs
using Microsoft.Extensions.Localization;
using System.Globalization;
using Microsoft.Extensions.Logging;
using System.Net.Http;

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

            // Configurar HttpClient correctamente
            builder.Services.AddHttpClient<ApiService>(client =>
            {
                client.BaseAddress = new Uri("https://appforseii2526dpj-api-djc2bgfag0h6gbby.francecentral-01.azurewebsites.net/");
                client.DefaultRequestHeaders.Add("User-Agent", "MauiApp");
            });

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}