namespace CyberSiberiaApp.Views;

public partial class FlatsPage : ContentPage
{
	public FlatsPage(int facility)
	{
		InitializeComponent();
		VM.GetNavigation(Navigation);
		VM.FacilityId = facility;
		VM.UpdateFlats();
	}
}