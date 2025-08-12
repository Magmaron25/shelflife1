namespace shelflife.app;

/// <summary>
/// Holds constants used throughout the app such as API base URL.  The
/// BaseUrl must point to the backend API.  When running on an Android
/// emulator, use 10.0.2.2 to access the host machine.
/// </summary>
public static class Constants
{
#if ANDROID
    public const string BaseUrl = "http://10.0.2.2:5186"; // adjust port to match API
#else
    public const string BaseUrl = "http://localhost:5186";
#endif
}