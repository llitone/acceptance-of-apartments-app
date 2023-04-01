using CyberSiberiaApp.Model.DB;
using CyberSiberiaApp.Views;

namespace CyberSiberiaApp;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
        VM.GetNavigation(Navigation);
    }
}

