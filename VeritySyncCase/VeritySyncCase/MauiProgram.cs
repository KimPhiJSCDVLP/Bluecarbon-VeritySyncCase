
using VeritySyncCase.Service;
using VeritySyncCase.View;
using VeritySyncCase.ViewModels;

namespace VeritySyncCase;

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
			});
		
        builder.Services.AddTransient<WelcomePage>();
        builder.Services.AddTransient<WelcomePageViewModel>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        return builder.Build();
	}
}
