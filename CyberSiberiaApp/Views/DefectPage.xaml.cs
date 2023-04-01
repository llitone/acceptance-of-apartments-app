namespace CyberSiberiaApp.Views;

public partial class DefectPage : ContentPage
{
	public DefectPage(int defectId)
	{
		InitializeComponent();
		VM.DefectId = defectId;
		VM.UpdateDefect();
	}
}