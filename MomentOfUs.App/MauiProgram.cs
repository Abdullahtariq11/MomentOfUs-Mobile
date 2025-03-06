using Microsoft.Extensions.Logging;
using MomentOfUs.App.Services;
using MomentOfUs.App.ViewModels;
using MomentOfUs.App.Views;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace MomentOfUs.App;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("OpenPlaylist-Script.otf", "PlaylistScript");
            })
			.UseSkiaSharp();

        builder.Services.AddHttpClient();
        builder.Services.AddSingleton<AuthService>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<LoginPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
