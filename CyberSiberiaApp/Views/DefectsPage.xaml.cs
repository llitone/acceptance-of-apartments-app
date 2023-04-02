using CyberSiberiaApp.ViewModels;

namespace CyberSiberiaApp.Views;

public partial class DefectsPage : ContentPage
{
	private FlatsViewModel _viewModel;
	public DefectsPage(FlatsViewModel viewModel, int flatId)
	{
		InitializeComponent();
		_viewModel = viewModel;
		VM.FlatId = flatId;
        VM.Close += VM_Close;
		VM.GetNavigation(Navigation);
		VM.UpdateDefects();
	}

    private void VM_Close()
    {
		_viewModel.UpdateFlats();
		Navigation.RemovePage(this);
    }
}