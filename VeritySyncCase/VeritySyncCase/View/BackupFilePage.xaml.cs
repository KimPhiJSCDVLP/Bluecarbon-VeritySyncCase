using Microsoft.Maui.Controls;
using SharpAdbClient;
using VeritySyncCase.Models;

namespace VeritySyncCase.View;

public partial class BackupFilePage : ContentPage
{
    public ConcurrentObservableCollection<BackupFileDTO> ItemsList { get; set; }
    public BackupFilePage()
	{
		InitializeComponent();
        ItemsList = new ConcurrentObservableCollection<BackupFileDTO>();
        LoadData();
        BindingContext = this;
    }

    private void LoadData()
    {
        var f1 = new BackupFileDTO()
        {
            BackupFileName = "Backup_023_31_05_2023",
            UpdatedDate = DateTime.UtcNow,
            Device = new DeviceDataDTO()
            {
                Model = "Tab S8",
                Name = "Samsung galaxy Tab S8",
                Product = "aa",
                Serial = "88191FFAZ004ZHBB",
                State = DeviceState.Offline,
                IsOnline = false,
                IsShowWarning = true,
                TransportId = "36",
            }
        };
        var f2 = new BackupFileDTO()
        {
            BackupFileName = "Backup_023_28_02_2023",
            UpdatedDate = DateTime.UtcNow,
            Device = new DeviceDataDTO()
            {
                Model = "Tab S8",
                Name = "Samsung galaxy Tab S8",
                Product = "aa",
                Serial = "88191FFAZ004ZHBB",
                State = DeviceState.Offline,
                IsOnline = false,
                IsShowWarning = true,
                TransportId = "36",
            }
        };
        var f3 = new BackupFileDTO()
        {
            BackupFileName = "Backup_023_30_10_2023",
            UpdatedDate = DateTime.UtcNow,
            Device = new DeviceDataDTO()
            {
                Model = "Tab S8",
                Name = "Samsung galaxy Tab S8",
                Product = "aa",
                Serial = "88191FFAZ004ZHBB",
                State = DeviceState.Offline,
                IsOnline = false,
                IsShowWarning = true,
                TransportId = "36",
            }
        };
        var f4 = new BackupFileDTO()
        {
            BackupFileName = "Backup_023_31_04_2023",
            UpdatedDate = DateTime.UtcNow,
            Device = new DeviceDataDTO()
            {
                Model = "Tab S8",
                Name = "Samsung galaxy Tab S8",
                Product = "aa",
                Serial = "88191FFAZ004ZHBB",
                State = DeviceState.Offline,
                IsOnline = false,
                IsShowWarning = true,
                TransportId = "36",
            }
        };
        var f5 = new BackupFileDTO()
        {
            BackupFileName = "Backup_023_30_02_2023",
            UpdatedDate = DateTime.UtcNow,
            Device = new DeviceDataDTO()
            {
                Model = "Tab A8",
                Name = "Samsung galaxy Tab A8",
                Product = "aa",
                Serial = "88191FFAZ004ZHBB",
                State = DeviceState.Offline,
                IsOnline = false,
                IsShowWarning = true,
                TransportId = "36",
            }
        };
        var f6 = new BackupFileDTO()
        {
            BackupFileName = "Backup_023_31_01_2023",
            UpdatedDate = DateTime.UtcNow,
            Device = new DeviceDataDTO()
            {
                Model = "Tab S7",
                Name = "Samsung galaxy Tab S7",
                Product = "aa",
                Serial = "88191FFAZ004ZHBB",
                State = DeviceState.Offline,
                IsOnline = false,
                IsShowWarning = true,
                TransportId = "36",
            }

        };
        ItemsList.Add(f1);
        ItemsList.Add(f2);
        ItemsList.Add(f3);
        ItemsList.Add(f4);
        ItemsList.Add(f5);
        ItemsList.Add(f6);
    }
}