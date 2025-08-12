namespace shelflife.app;

/// <summary>
/// Application bootstrapper.  Sets the main page to the shell for navigation.
/// </summary>
public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }
}