namespace shelflife.DTO;

/// <summary>
/// Data transfer object used when updating an existing item.  Matches
/// the creation DTO but excludes read-only fields such as Id and timestamps.
/// </summary>
public class DtoUpdateItem
{
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; } = 1;
    public DateTime? ExpiryDate { get; set; }
    public string? Location { get; set; }
    public string? Barcode { get; set; }
    public string? Notes { get; set; }
}