using CyberSiberiaApp.ViewModels;

namespace CyberSiberiaApp.Views;

public partial class AddDefectPage : ContentPage
{
    DefectsViewModel _viewModel;
	public AddDefectPage(DefectsViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        VM.Close += VM_Close;
		VM.FlatId = _viewModel.FlatId;
	}

    private void VM_Close()
    {
        _viewModel.UpdateDefects();
        Navigation.RemovePage(this);
    }
}