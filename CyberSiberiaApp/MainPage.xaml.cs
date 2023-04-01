using CyberSiberiaApp.Model.DB;

namespace CyberSiberiaApp;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
    }

	private void OnCounterClicked(object sender, EventArgs e)
	{
        count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		using(Context context = new Context())
		{
			context.Facilities.Add(new Model.DB.EntityModels.Facility()
			{
				Address = "Советская 63"
			});
			context.SaveChanges();
			label.Text = context.Facilities.ToList().Count.ToString();
		}


		SemanticScreenReader.Announce(CounterBtn.Text);
	}
}

