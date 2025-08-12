namespace shelflife.DTO;

/// <summary>
/// Data transfer object used when creating a new item.  Only fields that
/// the client may supply are included here.  Identifiers and timestamps
/// are populated by the server.
/// </summary>
public class DtoCreateItem
{
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; } = 1;
    public DateTime? ExpiryDate { get; set; }
    public string? Location { get; set; }
    public string? Barcode { get; set; }
    public string? Notes { get; set; }
}