using shelflife.app.ViewModels;

namespace shelflife.app.Views;

/// <summary>
/// Code-behind for <see cref="ItemsPage"/>.  Sets the binding context and
/// triggers an initial load of items on appearing.
/// </summary>
public partial class ItemsPage : ContentPage
{
    private readonly ItemsVM _vm;

    public ItemsPage(ItemsVM vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Trigger data load when the page appears.  Use the command rather
        // than directly calling the method to respect command state.
        if (_vm.LoadCommand.CanExecute(null))
        {
            _vm.LoadCommand.Execute(null);
        }
    }
}