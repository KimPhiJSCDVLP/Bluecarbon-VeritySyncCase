
namespace VeritySyncCase.Models
{
    public class BackupFileDTO
    {
        public string BackupFileName { get; set; }

        public DeviceDataDTO device { get; set; }

        public DateTime UpdatedDate { get; set; }

    }

}
