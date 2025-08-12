using Microsoft.Extensions.Logging;
using shelflife.app.Services;
using shelflife.app.ViewModels;
using shelflife.app.Views;

namespace shelflife.app;

/// <summary>
/// Configures the MAUI application including dependency injection and fonts.
/// </summary>
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Register HttpClient for API calls.  On Android the BaseAddress will be
        // set by Constants.cs via the ApiClient constructor.
        builder.Services.AddHttpClient<IApiClient, ApiClient>();

        // Register view models and pages.  Using transient here ensures a
        // separate instance is created each time the page is navigated to.
        builder.Services.AddTransient<ItemsVM>();
        builder.Services.AddTransient<ItemsPage>();
        builder.Services.AddTransient<EditItemVM>();
        builder.Services.AddTransient<EditItemPage>();

        return builder.Build();
    }
}