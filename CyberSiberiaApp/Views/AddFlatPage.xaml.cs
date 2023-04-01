using CyberSiberiaApp.ViewModels;

namespace CyberSiberiaApp.Views;

public partial class AddFlatPage : ContentPage
{
    FlatsViewModel _flatsVM;
	public AddFlatPage(FlatsViewModel flatsVM)
	{
		InitializeComponent();
        _flatsVM = flatsVM;
        VM.Close += VM_Close;
        VM.FacilityId = _flatsVM.FacilityId;
	}

    private void VM_Close()
    {
		_flatsVM.UpdateFlats();
        Navigation.RemovePage(this);
    }
}