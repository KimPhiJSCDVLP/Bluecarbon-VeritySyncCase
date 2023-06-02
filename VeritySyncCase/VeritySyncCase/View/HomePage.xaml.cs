using VeritySyncCase.Models;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Documents;

#if WINDOWS
using VeritySyncCase.Utils;
using System.Management;
using VeritySyncCase.Constants;
using Windows.Devices.Enumeration;
using SharpAdbClient;
#endif

namespace VeritySyncCase.View;

public partial class HomePage : ContentPage
{
    public HomePage()
	{
        InitializeComponent();
#if WINDOWS
        Devices = new ConcurrentObservableCollection<DeviceDataDTO>();
        LoadConnectedMobileDevices();
        LoadData();
        StartMonitoring();
        StartWatching();
        BindingContext = this;
#endif
    }

    private void ViewCell_Appearing(object sender, EventArgs e)
    {
    }
#if WINDOWS
    public ConcurrentObservableCollection<DeviceDataDTO> Devices { get; set; }
private void LoadData()
    {
		var device1 = new DeviceDataDTO() { 
			Model = "Tab S8",
			Name = "Samsung galaxy Tab S8",
			Product= "aa",
			Serial = "88191FFAZ004ZHBB",
			State = DeviceState.Offline,
            IsOnline = false,
            IsShowWarning = true,
            TransportId = "36",
        };
        var device2 = new DeviceDataDTO()
        {
            Model = "SS22",
            Name = "Samsung galaxy SS22",
            Product = "aa",
            Serial = "99191FFAZ004ZHAA",
            State = DeviceState.Online,
            IsOnline = true,
            IsShowWarning = false,
            TransportId = "36"
        };
        var device3 = new DeviceDataDTO()
        {
            Model = "Pixel_4",
            Name = "Name",
            Product = "aa",
            Serial = "99191FFAZ004ZHWS",
            State = DeviceState.Offline,
            IsOnline = false,
            IsShowWarning = true,
            TransportId = "36"
        };
        var device4 = new DeviceDataDTO()
        {
            Model = "Pixel_4",
            Name = "Samsung galaxy Tab A8",
            Product = "aa",
            Serial = "77191FFAZ004ZH",
            State = DeviceState.Offline,
            IsOnline = false,
            IsShowWarning = true,
            TransportId = "36"
        };
        var device5 = new DeviceDataDTO()
        {
            Model = "Tab S7+",
            Name = "Samsung galaxy Tab S7+",
            Product = "aa",
            Serial = "22191FFAZ004ZH",
            State = DeviceState.Offline,
            IsOnline = false,
            IsShowWarning = true,
            TransportId = "36"
        };
        var device6 = new DeviceDataDTO()
        {
            Model = "Tab S6",
            Name = "Samsung galaxy Tab S6",
            Product = "aa",
            Serial = "33191FFAZ004ZH",
            State = DeviceState.Offline,
            IsOnline = false,
            IsShowWarning = true,
            TransportId = "36"
        };
        Devices.Add(device1);
        Devices.Add(device2);
        Devices.Add(device3);
        Devices.Add(device4);
        Devices.Add(device5);
        Devices.Add(device6);
    }

    private ManagementEventWatcher watcher;
    private DeviceWatcher deviceWatcher;

    public void StartWatching()
    {
        string deviceSelector = "System.Devices.InterfaceClassGuid:=\"{A5DCBF10-6530-11D2-901F-00C04FB951ED}\" AND System.Devices.InterfaceEnabled:=System.StructuredQueryType.Boolean#True";
        deviceWatcher = DeviceInformation.CreateWatcher(deviceSelector);
        deviceWatcher.Removed += DeviceRemoved;
        deviceWatcher.Start();
    }

    private void DeviceRemoved(DeviceWatcher sender, DeviceInformationUpdate deviceInfoUpdate)
    {
        LoadConnectedMobileDevices();
    }

    async void OnSyncButtonClicked(object sender, EventArgs args)
    {
        //if (IsDeviceConnected(deviceData))
        //{
        //}
        var deviceId = Devices.Select(x => x.Serial).FirstOrDefault();
        if (deviceId != null)
        {
            var drives = DriveInfo.GetDrives();
            var destinationPaths = new List<string>();
            foreach (var driver in drives)
            {
                var destinationPath = Path.Combine(driver.Name, Constant.SUB_FOLDER);
                FileHelper.CreateFolder(destinationPath);
                destinationPaths.Add(destinationPath);
            }
            await AdbHelper.PullFilesFromDevices(deviceId, destinationPaths);
        }
        LoadConnectedMobileDevices();
    }

    private void LoadConnectedMobileDevices()
    {
        var devices = AdbHelper.GetDevices();
        if (Devices.Count == 0)
        {
            foreach (var item in devices)
            {
                var deviceDataDTO = new DeviceDataDTO()
                {
                    Model = item.Model,
                    Name = item.Name,
                    Product = item.Product,
                    Serial = item.Serial,
                    State = item.State,
                    TransportId = item.TransportId,
                    IsOnline = item.State == DeviceState.Online ? true : false,
                    IsShowWarning = item.State == DeviceState.Online ? false : true
                };
                Devices.Add(deviceDataDTO);
            }
        }
        else
        {
            foreach (var item in Devices)
            {
                item.State = DeviceState.Offline;
                item.IsOnline = false;
                item.IsShowWarning = true;
            }
            foreach (var item in devices)
            {
                var existDevice = Devices.FirstOrDefault(x => x.Serial == item.Serial);
                if (existDevice == null)
                {
                    var deviceDataDTO = new DeviceDataDTO()
                    {
                        Model = item.Model,
                        Name = item.Name,
                        Product = item.Product,
                        Serial = item.Serial,
                        State = item.State,
                        TransportId = item.TransportId,
                        IsOnline = item.State == DeviceState.Online ? true : false,
                        IsShowWarning = item.State == DeviceState.Online ? false : true
                    };
                    Devices.Add(deviceDataDTO);
                }
                else
                {
                    existDevice.IsOnline = item.State == DeviceState.Online ? true : false;
                    existDevice.IsShowWarning = item.State == DeviceState.Online ? false : true;
                    existDevice.State = item.State;
                }
            }
        }
    }
    private void StartMonitoring()
    {
        WqlEventQuery query = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_USBControllerDevice'");
        ManagementScope scope = new ManagementScope("root\\cimv2");
        ManagementEventWatcher watcher = new ManagementEventWatcher(scope, query);
        watcher.EventArrived += Watcher_EventArrived;
        watcher.Start();
    }
    private void Watcher_EventArrived(object sender, EventArrivedEventArgs e)
    {
        LoadConnectedMobileDevices();
    }
    private bool IsDeviceConnected(DeviceData deviceData)
    {
        var devices = AdbHelper.GetDevices();
        var device = devices.FirstOrDefault(x => x.Serial == deviceData.Serial);
        deviceData.State = device.State;
        return (device != null && device.State == DeviceState.Online) ? true : false;
    }
#else
    async void OnSyncButtonClicked(object sender, EventArgs args)
    {
    }
#endif
}