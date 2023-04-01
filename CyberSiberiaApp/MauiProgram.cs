using CyberSiberiaApp.Model.DB;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace CyberSiberiaApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		using(Context context = new())
		{
			if(context.Categories.ToList().Count == 0)
			{
				foreach(var category in CategoriesList.Categories)
				{
					context.Categories.Add(category);
				}
				context.SaveChanges();
			}
		}
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
