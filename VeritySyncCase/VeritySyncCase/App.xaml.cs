using VeritySyncCase.View;

namespace VeritySyncCase;

public partial class App : Application
{
    public App()
	{
		InitializeComponent();

		MainPage = new HomeMainPage();
	}
}
