using shelflife.app.ViewModels;

namespace shelflife.app.Views;

/// <summary>
/// Code-behind for <see cref="EditItemPage"/>.  Sets the binding context
/// via constructor injection and calls the Appearing command when the
/// page is displayed so that data is loaded if editing.
/// </summary>
public partial class EditItemPage : ContentPage
{
    private readonly EditItemVM _vm;

    public EditItemPage(EditItemVM vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (_vm.AppearingCommand.CanExecute(null))
        {
            _vm.AppearingCommand.Execute(null);
        }
    }
}