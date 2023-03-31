﻿using Microsoft.Extensions.Logging;

namespace CyberSiberiaApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
        //ImageLoader imageLoader = new ImageLoader();
        //Console.WriteLine(imageLoader.convert_image("IMG_4149.heic"));
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