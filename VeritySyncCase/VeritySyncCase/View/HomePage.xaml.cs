using SharpAdbClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VeritySyncCase.Models;

namespace VeritySyncCase.View;

public partial class HomePage : ContentPage
{
    ObservableCollection<DeviceDataDTO> fruits = new ObservableCollection<DeviceDataDTO>();
    public ObservableCollection<DeviceDataDTO> Fruits { get { return fruits; } }
    public HomePage()
	{
        InitializeComponent();
        LoadData();
        FruitListView.ItemsSource = fruits;
    }

    private void LoadData()
    {
		var dev1 = new DeviceDataDTO() { 
			Model = "Pixel_4",
			Name = "Samsung galaxy Tab S8",
			Product= "aa",
			Serial = "99191FFAZ004ZH",
			State = DeviceState.Online,
			TransportId = "36",
        };
        var dev2 = new DeviceDataDTO()
        {
            Model = "Pixel_4",
            Name = "Samsung galaxy SS22",
            Product = "aa",
            Serial = "99191FFAZ004ZH",
            State = DeviceState.Online,
            TransportId = "36"
        };
        var dev3 = new DeviceDataDTO()
        {
            Model = "Pixel_4",
            Name = "Name",
            Product = "aa",
            Serial = "99191FFAZ004ZH",
            State = DeviceState.Offline,
            TransportId = "36"
        };
        var dev4 = new DeviceDataDTO()
        {
            Model = "Pixel_4",
            Name = "Samsung galaxy Tab A8",
            Product = "aa",
            Serial = "99191FFAZ004ZH",
            State = DeviceState.Online,
            TransportId = "36"
        };
        var dev5 = new DeviceDataDTO()
        {
            Model = "Pixel_4",
            Name = "Samsung galaxy Tab S7+",
            Product = "aa",
            Serial = "99191FFAZ004ZH",
            State = DeviceState.Offline,
            TransportId = "36"
        };
        var dev6 = new DeviceDataDTO()
        {
            Model = "Pixel_4",
            Name = "Samsung galaxy Tab S6",
            Product = "aa",
            Serial = "99191FFAZ004ZH",
            State = DeviceState.Offline,
            TransportId = "36"
        };
        fruits.Add(dev1);
        fruits.Add(dev2);
        fruits.Add(dev3);
        fruits.Add(dev4);
        fruits.Add(dev5);
        fruits.Add(dev6);
    }

    private void ViewCell_Appearing(object sender, EventArgs e)
    {

    }
}