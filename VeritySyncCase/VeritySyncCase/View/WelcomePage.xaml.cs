#if WINDOWS
using VeritySyncCase.Utils;
using System.Management;
using SharpAdbClient;
using VeritySyncCase.Constants;
using Windows.Devices.Enumeration;
#endif
namespace VeritySyncCase.View;

public partial class WelcomePage : ContentPage
{
#if WINDOWS
    private System.Timers.Timer connectionCheckTimer;
    private ManagementEventWatcher watcher;
    public List<DeviceData> mobileDeviceList;
#endif
    public WelcomePage()
	{
		InitializeComponent();
#if WINDOWS
        mobileDeviceList = new List<DeviceData>();
        LoadConnectedMobileDevices();
        StartMonitoring();
        StartWatching();
#endif
    }
#if WINDOWS

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
        var deviceId = mobileDeviceList.Select(x => x.Serial).FirstOrDefault();
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
        if (mobileDeviceList.Count == 0)
            mobileDeviceList.AddRange(devices);
        else
        {
            foreach (var item in mobileDeviceList)
            {
                item.State = DeviceState.Offline;
            }
            foreach (var item in devices)
            {
                var existDevice = mobileDeviceList.FirstOrDefault(x => x.Serial == item.Serial);
                if (existDevice == null)
                    mobileDeviceList.Add(item);
                else
                    existDevice.State = item.State;
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
        return(device != null && device.State == DeviceState.Online) ?  true : false;
    }
#else
    async void OnSyncButtonClicked(object sender, EventArgs args)
    {
    }
#endif
}