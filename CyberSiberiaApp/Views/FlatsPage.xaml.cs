using CyberSiberiaApp.ViewModels;

namespace CyberSiberiaApp.Views;

public partial class FlatsPage : ContentPage
{
	MainViewModel _viewModel;
	public FlatsPage(MainViewModel viewModel, int facility)
	{
		InitializeComponent();
		_viewModel = viewModel;
		VM.GetNavigation(Navigation);
        VM.Close += VM_Close;
		VM.FacilityId = facility;
		VM.UpdateFlats();
	}

    private void VM_Close()
    {
		_viewModel.UpdateFacilities();
        Navigation.RemovePage(this);
    }
}