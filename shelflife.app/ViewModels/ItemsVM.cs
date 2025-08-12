using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using shelflife.app.Models;
using shelflife.app.Services;
using System.Collections.ObjectModel;
using shelflife.DTO;

namespace shelflife.app.ViewModels;

/// <summary>
/// ViewModel for the items list page.  Manages loading items from the
/// backend, filtering by search term, and navigating to edit or create
/// pages.  Uses CommunityToolkit.Mvvm for observable properties and
/// commands.
/// </summary>
public partial class ItemsVM : ObservableObject
{
    private readonly IApiClient _api;

    public ItemsVM(IApiClient api)
    {
        _api = api;
    }

    /// <summary>
    /// The collection of items displayed in the UI.  It is replaced
    /// wholesale whenever data is loaded.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<ItemModel> items = new();

    /// <summary>
    /// Search term bound to the SearchBar.  Changing this property does
    /// not automatically reload; the SearchCommand of the SearchBar
    /// triggers the reload.
    /// </summary>
    [ObservableProperty]
    private string? search;

    /// <summary>
    /// Indicates whether a refresh is in progress.  Bound to the
    /// RefreshView's IsRefreshing property.
    /// </summary>
    [ObservableProperty]
    private bool isRefreshing;

    /// <summary>
    /// Called by the view when it appears for the first time or when the
    /// user performs a pull‑to‑refresh.  Loads items from the backend
    /// respecting the search term.
    /// </summary>
    [RelayCommand]
    private async Task Load()
    {
        IsRefreshing = true;
        var list = await _api.GetItems(Search);
        Items = new ObservableCollection<ItemModel>(list.Select(Map));
        IsRefreshing = false;
    }

    /// <summary>
    /// Navigates to the EditItemPage without an Id, indicating creation
    /// of a new item.
    /// </summary>
    [RelayCommand]
    private async Task AddNew()
    {
        await Shell.Current.GoToAsync(nameof(EditItemPage));
    }

    /// <summary>
    /// Navigates to the EditItemPage for the specified item.
    /// </summary>
    /// <param name="item">The item to edit.</param>
    [RelayCommand]
    private async Task Edit(ItemModel? item)
    {
        if (item == null) return;
        await Shell.Current.GoToAsync($"{nameof(EditItemPage)}?Id={item.Id}");
    }

    /// <summary>
    /// Deletes the specified item after confirming with the user.
    /// </summary>
    /// <param name="item">The item to delete.</param>
    [RelayCommand]
    private async Task Delete(ItemModel? item)
    {
        if (item == null) return;
        bool confirm = await Shell.Current.DisplayAlert(
            "Delete",
            $"Delete '{item.Name}'?",
            "Yes",
            "No");
        if (!confirm) return;
        var success = await _api.DeleteItem(item.Id);
        if (success)
        {
            await Load();
        }
        else
        {
            await Shell.Current.DisplayAlert("Error", "Could not delete item.", "OK");
        }
    }

    /// <summary>
    /// Maps a DtoItem to the UI model.  Computes ExpiryPretty via the
    /// ItemModel property.
    /// </summary>
    private static ItemModel Map(DtoItem dto) => new()
    {
        Id = dto.Id,
        Name = dto.Name,
        Quantity = dto.Quantity,
        ExpiryDate = dto.ExpiryDate,
        Location = dto.Location,
        Barcode = dto.Barcode,
        Notes = dto.Notes
    };
}