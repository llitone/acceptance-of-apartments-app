namespace CyberSiberiaApp.Views;

public partial class DefectsPage : ContentPage
{
	public DefectsPage(int flatId)
	{
		InitializeComponent();
		VM.FlatId = flatId;
		VM.GetNavigation(Navigation);
		VM.UpdateDefects();
	}
}