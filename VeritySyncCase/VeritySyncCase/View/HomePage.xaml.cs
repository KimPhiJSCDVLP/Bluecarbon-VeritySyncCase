using System.Xml.Linq;
using VeritySyncCase.Models;
using VeritySyncCase.ViewModels;

namespace VeritySyncCase.View;

public partial class HomePage : ContentPage
{
    public HomePageViewModel viewModel { get; set; }
    public HomePage()
	{
        InitializeComponent();
        this.viewModel = (HomePageViewModel)BindingContext;
        DeviceListView.ItemsSource = viewModel.Devices;
    }
}