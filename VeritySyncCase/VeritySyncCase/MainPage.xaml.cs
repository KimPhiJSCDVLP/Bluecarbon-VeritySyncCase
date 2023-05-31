
#if WINDOWS
using VeritySyncCase.Models;
using System.Management;
using Windows.Devices.Enumeration;
using Windows.Management.Deployment;
#endif

namespace VeritySyncCase;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

#if WINDOWS
        mobileDeviceList = new List<DeviceInfoDto>();
        var devices = DriveInfo.GetDrives();
        LoadConnectedMobileDevices();
        Thread backgroundThread = new Thread(DeviceConnectionTask);
        backgroundThread.IsBackground = true;
        backgroundThread.Start();
#endif
    }

#if WINDOWS
    private ManagementEventWatcher watcher;
    public static List<DeviceInfoDto> mobileDeviceList;
    async void OnSyncButtonClicked(object sender, EventArgs args)
    {
        foreach (var item in mobileDeviceList)
        {
            //var output = AdbHelper.GetFilePathInDocuments("newtext.txt", item.DeviceName);
        }
    }

    private void DeviceConnectionTask()
    {
        // Create a WMI query to monitor device connection events
        WqlEventQuery query = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_USBControllerDevice'");

        // Create a management scope to connect to the root\cimv2 namespace
        ManagementScope scope = new ManagementScope("root\\cimv2");

        // Create a watcher to listen for events
        ManagementEventWatcher watcher = new ManagementEventWatcher(scope, query);

        // Subscribe to the event arrived event
        watcher.EventArrived += (sender, e) =>
        {
            // Extract the target instance from the event arguments
            ManagementBaseObject targetInstance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            ManagementObject dependent = new ManagementObject((string)targetInstance.GetPropertyValue("Dependent"));
            string deviceName = dependent["Name"]?.ToString();
            string deviceId = dependent["DeviceID"]?.ToString();
            string deviceManufacturer = dependent["Manufacturer"]?.ToString();

            // Check if the device is a mobile device based on the manufacturer or any other criteria
            if (!string.IsNullOrEmpty(deviceName) && !string.IsNullOrEmpty(deviceManufacturer) && IsMobileDevice(deviceManufacturer))
            {
                //var installed = CheckAppInstalled("com.example.blue_carbon", deviceName);
                var existDevice = mobileDeviceList.FirstOrDefault(x => x.DeviceId == deviceId);
                if (existDevice == null)
                {
                    var deviceInfoDto = new DeviceInfoDto();
                    deviceInfoDto.DeviceId = deviceId;
                    deviceInfoDto.DeviceName = deviceName;
                    deviceInfoDto.Manufacturer = deviceManufacturer;
                    mobileDeviceList.Add(deviceInfoDto);
                }
            }
        };

        // Start listening for events
        watcher.Start();

        // Keep the background task running until the main thread exits
        while (true)
        {
            Thread.Sleep(1000);
        }
    }

    private async Task<bool> CheckAppInstalled(string packageName, string deviceName)
    {
        var devices = await DeviceInformation.FindAllAsync();
        var device = devices.FirstOrDefault(d => d.Name.Equals(deviceName));

        if (device != null)
            return CheckAppInstalledOnDevice(device, packageName);
        return false;

    }
    private bool CheckAppInstalledOnDevice(DeviceInformation device, string packageId)
    {
        try
        {
            PackageManager packageManager = new PackageManager();
            var packages = packageManager.FindPackages(device.Id);
            return packages.Any(package => package.Id.FamilyName.Equals(packageId));
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    private void LoadConnectedMobileDevices()
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_USBControllerDevice");

        foreach (ManagementObject obj in searcher.Get())
        {
            ManagementObject dependent = new ManagementObject((string)obj["Dependent"]);
            string deviceId = dependent["DeviceID"]?.ToString();
            string deviceName = dependent["Name"]?.ToString();
            string deviceManufacturer = dependent["Manufacturer"]?.ToString();

            // Check if the device is a mobile device based on the manufacturer or any other criteria
            if (!string.IsNullOrEmpty(deviceName) && !string.IsNullOrEmpty(deviceManufacturer) && IsMobileDevice(deviceManufacturer))
            {
                var existDevice = mobileDeviceList.FirstOrDefault(x => x.DeviceId == deviceId);
                if (existDevice == null)
                {
                    var deviceInfoDto = new DeviceInfoDto();
                    deviceInfoDto.DeviceId = deviceId;
                    deviceInfoDto.DeviceName = deviceName;
                    deviceInfoDto.Manufacturer = deviceManufacturer;
                    mobileDeviceList.Add(deviceInfoDto);
                }
            }
        }
    }

    private bool IsMobileDevice(string manufacturer)
    {
        string[] mobileKeywords = { "Samsung", "Apple", "Tablet", "Google", "OnePlus", "Xiaomi", "Huawei", "LG", "Motorola" };
        return mobileKeywords.Any(keyword => manufacturer.Contains(keyword, StringComparison.OrdinalIgnoreCase));
    }
#endif
}
