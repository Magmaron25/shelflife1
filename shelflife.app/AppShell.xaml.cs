using shelflife.app.Views;

namespace shelflife.app;

/// <summary>
/// Shell controlling the overall navigation structure of the app.  Routes
/// for secondary pages are registered here so that Shell navigation
/// functions correctly.
/// </summary>
public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(EditItemPage), typeof(EditItemPage));
    }
}