using CyberSiberiaApp.ViewModels;

namespace CyberSiberiaApp.Views;

public partial class AddFacilityPage : ContentPage
{
	MainViewModel _main;
	public AddFacilityPage(MainViewModel main)
	{
		InitializeComponent();
		VM.Close += Close;
		_main = main;
	}

    private void Close()
    {
		_main.UpdateFacilities();
        Navigation.RemovePage(this);
    }
}