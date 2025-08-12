namespace shelflife.app.Models;

/// <summary>
/// View model class used to represent an item within the app's UI.  Provides
/// convenient computed properties for display such as a friendly expiry
/// string.
/// </summary>
public class ItemModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? Location { get; set; }
    public string? Barcode { get; set; }
    public string? Notes { get; set; }
    public string ExpiryPretty => ExpiryDate?.ToLocalTime().ToString("dddd, MMMM dd, yyyy") ?? "—";
}