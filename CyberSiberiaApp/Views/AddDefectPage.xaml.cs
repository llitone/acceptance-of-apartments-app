namespace CyberSiberiaApp.Views;

public partial class AddDefectPage : ContentPage
{
	public AddDefectPage(int flatId)
	{
		InitializeComponent();
        VM.Close += VM_Close;
		VM.FlatId = flatId;
	}

    private void VM_Close()
    {
        Navigation.RemovePage(this);
    }
}