using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using shelflife.app.Services;
using shelflife.DTO;

namespace shelflife.app.ViewModels;

/// <summary>
/// ViewModel used for both creating and editing items.  When the Id
/// property is non‑zero the ViewModel loads the existing data from the
/// backend on appearing.  Saves send either a POST or PUT depending on
/// whether Id is set.
/// </summary>
[QueryProperty(nameof(Id), "Id")]
public partial class EditItemVM : ObservableObject
{
    private readonly IApiClient _api;

    public EditItemVM(IApiClient api)
    {
        _api = api;
        // Defaults for a new item
        Quantity = 1;
        Date = DateTime.Today;
    }

    // Id bound via QueryProperty.  A value of zero means create mode.
    [ObservableProperty]
    private int id;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private int quantity = 1;

    [ObservableProperty]
    private DateTime? date;

    [ObservableProperty]
    private string? location;

    [ObservableProperty]
    private string? barcode;

    [ObservableProperty]
    private string? notes;

    /// <summary>
    /// Called when the page appears.  If an Id has been supplied it
    /// attempts to load the existing item details.
    /// </summary>
    [RelayCommand]
    private async Task Appearing()
    {
        if (Id <= 0)
        {
            // Creating a new item; nothing to load
            return;
        }
        var dto = await _api.GetItem(Id);
        if (dto != null)
        {
            Name = dto.Name;
            Quantity = dto.Quantity;
            // Convert UTC to local date for DatePicker
            Date = dto.ExpiryDate?.ToLocalTime().Date;
            Location = dto.Location;
            Barcode = dto.Barcode;
            Notes = dto.Notes;
        }
    }

    /// <summary>
    /// Saves the item.  Performs basic validation before sending data to
    /// the server.  After a successful save it navigates back to the
    /// previous page.
    /// </summary>
    [RelayCommand]
    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            await Shell.Current.DisplayAlert("Validation", "Name is required.", "OK");
            return;
        }
        if (Quantity <= 0)
        {
            await Shell.Current.DisplayAlert("Validation", "Quantity must be greater than zero.", "OK");
            return;
        }
        if (Id <= 0)
        {
            // Creating new item
            var newItem = new DtoCreateItem
            {
                Name = Name.Trim(),
                Quantity = Quantity,
                ExpiryDate = Date?.ToUniversalTime(),
                Location = Location?.Trim(),
                Barcode = Barcode?.Trim(),
                Notes = Notes?.Trim()
            };
            var id = await _api.CreateItem(newItem);
            if (id == null)
            {
                await Shell.Current.DisplayAlert("Error", "Failed to create item.", "OK");
                return;
            }
        }
        else
        {
            // Updating existing item
            var update = new DtoUpdateItem
            {
                Name = Name.Trim(),
                Quantity = Quantity,
                ExpiryDate = Date?.ToUniversalTime(),
                Location = Location?.Trim(),
                Barcode = Barcode?.Trim(),
                Notes = Notes?.Trim()
            };
            var ok = await _api.UpdateItem(Id, update);
            if (!ok)
            {
                await Shell.Current.DisplayAlert("Error", "Failed to update item.", "OK");
                return;
            }
        }
        // Navigate back (pop) to the previous page
        await Shell.Current.GoToAsync("..");
    }
}