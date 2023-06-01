using VeritySyncCase.ViewModels;

namespace VeritySyncCase.View;

public partial class WelcomePage : ContentPage
{
	public WelcomePageViewModel viewModel { get; set; }
    public WelcomePage()
	{
		InitializeComponent();
        this.viewModel = (WelcomePageViewModel)BindingContext;
    }

    private void nextPage_Clicked(object sender, EventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new HomeMainPage());
    }
}