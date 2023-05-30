using Microsoft.UI.Xaml;

using System.Management;
using System.Security.Principal;
using VeritySyncCase.Platforms.Windows.Dto;
using Windows.Devices.Enumeration;
using Windows.Management.Deployment;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VeritySyncCase.WinUI;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : MauiWinUIApplication
{
    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    
    private ManagementEventWatcher watcher;
    List<DeviceInfoDto> mobileDeviceList;
    public App()
	{
		this.InitializeComponent();
        mobileDeviceList = new List<DeviceInfoDto>();
        // Create a new thread for the background task
        Thread backgroundThread = new Thread(DeviceConnectionTask);
        backgroundThread.IsBackground = true;
        backgroundThread.Start();
    }

    private const string SourceFolderPath = @"C:\Path\To\Source\Folder";
    private const string DestinationFolderPath = @"C:\Path\To\Destination\Folder";

    async void DeviceAdded(DeviceWatcher sender, DeviceInformation deviceInfo)
    {
        var folder = await StorageFolder.GetFolderFromPathAsync(SourceFolderPath);
        var file = await folder.GetFileAsync("YourFile.txt"); // Replace "YourFile.txt" with the actual file name

        var destinationFolder = await StorageFolder.GetFolderFromPathAsync(DestinationFolderPath);

        await file.CopyAsync(destinationFolder, "CopiedFile.txt", NameCollisionOption.GenerateUniqueName); // Replace "CopiedFile.txt" with the desired destination file name
    }

    private void DeviceConnectedEventArrived(object sender, EventArrivedEventArgs e)
    {
        string deviceInstanceName = e.NewEvent.Properties["Dependent"].Value.ToString();
        string deviceName = deviceInstanceName.Split('=')[1].Trim('\"');
        Console.WriteLine($"USB device connected: {deviceName}");
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
            string deviceId = (string)targetInstance.GetPropertyValue("Dependent");
            ManagementObject dependent = new ManagementObject((string)targetInstance.GetPropertyValue("Dependent"));
            string deviceName = dependent["Name"]?.ToString();
            string deviceManufacturer = dependent["Manufacturer"]?.ToString();

            // Check if the device is a mobile device based on the manufacturer or any other criteria
            if (!string.IsNullOrEmpty(deviceName) && !string.IsNullOrEmpty(deviceManufacturer) && IsMobileDevice(deviceManufacturer))
            {
                var installed = CheckAppInstalled("com.example.blue_carbon", deviceName);
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
            // Access the properties of the target instance
            Console.WriteLine("Device connected: " + deviceName);
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
        PackageManager packageManager = new PackageManager();
        var packages = packageManager.FindPackages(device.Id);
        return packages.Any(package => package.Id.FamilyName.Equals(packageId));

    }

    //var devices = DriveInfo.GetDrives();
    //var deviceList = GetAllConnectedMobileDevices();
    //foreach (var deviceName in deviceList)
    //{
    //    var serialNumber = GetDeviceSerialNumber(deviceName);
    //    IsAppInstalled(serialNumber, "com.example.yourapp");
    //}
    //foreach (DriveInfo divinfo in devices)
    //{
    //    if (divinfo.IsReady)
    //    {
    //        Console.WriteLine(divinfo.DriveType);
    //    }
    //    else
    //        Console.WriteLine("Device not Ready");
    //}
    private List<string> GetAllConnectedMobileDevices()
    {
        List<string> mobileDeviceList = new List<string>();

        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_USBControllerDevice");

        foreach (ManagementObject obj in searcher.Get())
        {
            ManagementObject dependent = new ManagementObject((string)obj["Dependent"]);
            string deviceName = dependent["Name"]?.ToString();
            string deviceManufacturer = dependent["Manufacturer"]?.ToString();

            // Check if the device is a mobile device based on the manufacturer or any other criteria
            if (!string.IsNullOrEmpty(deviceName) && !string.IsNullOrEmpty(deviceManufacturer) && IsMobileDevice(deviceManufacturer))
            {
                mobileDeviceList.Add(deviceName);
            }
        }

        return mobileDeviceList;
    }

    private bool IsMobileDevice(string manufacturer)
    {
        string[] mobileKeywords = { "Samsung", "Apple", "Tablet", "Google", "OnePlus", "Xiaomi", "Huawei", "LG", "Motorola" };
        return mobileKeywords.Any(keyword => manufacturer.Contains(keyword, StringComparison.OrdinalIgnoreCase));
    }

    public string GetDeviceSerialNumber(string deviceName)
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity");
        foreach (ManagementObject obj in searcher.Get())
        {
            string name = obj["Name"]?.ToString();
            if (!string.IsNullOrEmpty(name) && name.Equals(deviceName, StringComparison.OrdinalIgnoreCase))
            {
                string serialNumber = obj["DeviceID"]?.ToString();
                return serialNumber;
            }
        }

        return null;
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}

