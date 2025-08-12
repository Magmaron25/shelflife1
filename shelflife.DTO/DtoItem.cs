namespace shelflife.DTO;

/// <summary>
/// Represents an item within the ShelfLife application.  This DTO is returned
/// from the API and contains all persisted fields including identifiers,
/// timestamps and optional properties such as location and barcode.
/// </summary>
public class DtoItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? Location { get; set; }
    public string? Barcode { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}