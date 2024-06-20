using Auth0.OidcClient;
using CommunityToolkit.Maui;
using Google.Cloud.Firestore.V1;
using Microsoft.Extensions.Logging;
using SchedBus.Pages;
using SchedBus.Services;
using SchedBus.ViewModels;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace SchedBus
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder.UseMauiApp<App>().UseSkiaSharp();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

                    fonts.AddFont("Brands-Regular-400.otf", "BRF");
                    fonts.AddFont("Free-Regular-400.otf", "FRF");
                    fonts.AddFont("Free-Solid-900.otf", "FSF");
                })
                .UseMauiMaps();

            builder.Services.AddSingleton<FirestoreService>();

            builder.Services.AddSingleton<PlansViewModel>();
            builder.Services.AddTransient<PlanEditViewModel>();
            builder.Services.AddTransient<TimeSetViewModel>();
            builder.Services.AddTransient<LocationSearchViewModel>();

            builder.Services.AddSingleton<PlanPage>();
            builder.Services.AddTransient<PlanEditPage>();
            builder.Services.AddTransient<TimeSetPage>();
            builder.Services.AddTransient<LocationSearch>();

            builder.Services.AddSingleton(
                new Auth0Client(
                    new()
                    {
                        // Domain = "<YOUR_AUTH0_DOMAIN>",
                        // ClientId = "<YOUR_CLIENT_ID>",
                        // RedirectUri = "myapp://callback/",
                        // PostLogoutRedirectUri = "myapp://callback/",
                        // Scope = "openid profile email"

                        Domain = "dev-sj30op2pm8vsumhe.us.auth0.com",
                        ClientId = "sC1fPj5YTFZAiT25wOKM81A5GpFjDXRO",
                        RedirectUri = "myapp://callback",
                        PostLogoutRedirectUri = "myapp://callback",
                        Scope = "openid profile email"
                    }
                )
            );

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
