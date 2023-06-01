using Microsoft.UI.Xaml.Documents;
using SharpAdbClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VeritySyncCase.Models;

namespace VeritySyncCase.View;

public partial class HomePage : ContentPage
{
    ObservableCollection<DeviceDataDTO> devices = new ObservableCollection<DeviceDataDTO>();
    public ObservableCollection<DeviceDataDTO> Devices { get { return devices; } }
    public HomePage()
	{
        InitializeComponent();
        LoadData();
        FruitListView.ItemsSource = devices;
    }

    private void LoadData()
    {
		var dev1 = new DeviceDataDTO() { 
			Model = "Tab S8",
			Name = "Samsung galaxy Tab S8",
			Product= "aa",
			Serial = "88191FFAZ004ZHBB",
			State = DeviceState.Online,
            TransportId = "36",
        };
        var dev2 = new DeviceDataDTO()
        {
            Model = "SS22",
            Name = "Samsung galaxy SS22",
            Product = "aa",
            Serial = "99191FFAZ004ZHAA",
            State = DeviceState.Online,
            TransportId = "36"
        };
        var dev3 = new DeviceDataDTO()
        {
            Model = "Pixel_4",
            Name = "Name",
            Product = "aa",
            Serial = "99191FFAZ004ZHWS",
            State = DeviceState.Offline,
            TransportId = "36"
        };
        var dev4 = new DeviceDataDTO()
        {
            Model = "Pixel_4",
            Name = "Samsung galaxy Tab A8",
            Product = "aa",
            Serial = "77191FFAZ004ZH",
            State = DeviceState.Online,
            TransportId = "36"
        };
        var dev5 = new DeviceDataDTO()
        {
            Model = "Tab S7+",
            Name = "Samsung galaxy Tab S7+",
            Product = "aa",
            Serial = "22191FFAZ004ZH",
            State = DeviceState.Offline,
            TransportId = "36"
        };
        var dev6 = new DeviceDataDTO()
        {
            Model = "Tab S6",
            Name = "Samsung galaxy Tab S6",
            Product = "aa",
            Serial = "33191FFAZ004ZH",
            State = DeviceState.Offline,
            TransportId = "36"
        };
        devices.Add(dev1);
        devices.Add(dev2);
        devices.Add(dev3);
        devices.Add(dev4);
        devices.Add(dev5);
        devices.Add(dev6);
    }

    private void ViewCell_Appearing(object sender, EventArgs e)
    {

    }
}