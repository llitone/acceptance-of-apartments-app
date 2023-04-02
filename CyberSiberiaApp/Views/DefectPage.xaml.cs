using CyberSiberiaApp.ViewModels;

namespace CyberSiberiaApp.Views;

public partial class DefectPage : ContentPage
{
	private DefectsViewModel _viewModel;
    public DefectPage(DefectsViewModel viewModel, int defectId)
	{
		InitializeComponent();
		_viewModel = viewModel;
        VM.Close += VM_Close;
		VM.DefectId = defectId;
		VM.UpdateDefect();
	}

    private void VM_Close()
    {
		_viewModel.UpdateDefects();
		Navigation.RemovePage(this);
    }
}