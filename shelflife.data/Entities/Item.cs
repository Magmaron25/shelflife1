namespace shelflife.data.Entities;

/// <summary>
/// Entity representing a pantry item in the database.  Fields are mirrored
/// across DTOs used by the API.  Timestamps are updated automatically by
/// the DbContext.
/// </summary>
public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; } = 1;
    public DateTime? ExpiryDate { get; set; }
    public string? Location { get; set; }
    public string? Barcode { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}