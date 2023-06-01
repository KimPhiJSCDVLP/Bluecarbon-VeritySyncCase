using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VeritySyncCase.Models
{
    public class DeviceDataDTO
    {
        internal const string DeviceDataRegexString = "^(?<serial>[a-zA-Z0-9_-]+(?:\\s?[\\.a-zA-Z0-9_-]+)?(?:\\:\\d{1,})?)\\s+(?<state>device|connecting|offline|unknown|bootloader|recovery|download|authorizing|unauthorized|host|no permissions)(?<message>.*?)(\\s+usb:(?<usb>[^:]+))?(?:\\s+product:(?<product>[^:]+))?(\\s+model\\:(?<model>[\\S]+))?(\\s+device\\:(?<device>[\\S]+))?(\\s+features:(?<features>[^:]+))?(\\s+transport_id:(?<transport_id>[^:]+))?$";

        private static readonly Regex Regex = new Regex("^(?<serial>[a-zA-Z0-9_-]+(?:\\s?[\\.a-zA-Z0-9_-]+)?(?:\\:\\d{1,})?)\\s+(?<state>device|connecting|offline|unknown|bootloader|recovery|download|authorizing|unauthorized|host|no permissions)(?<message>.*?)(\\s+usb:(?<usb>[^:]+))?(?:\\s+product:(?<product>[^:]+))?(\\s+model\\:(?<model>[\\S]+))?(\\s+device\\:(?<device>[\\S]+))?(\\s+features:(?<features>[^:]+))?(\\s+transport_id:(?<transport_id>[^:]+))?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public string Serial { get; set; }

        public DeviceState State { get; set; }

        public string Model { get; set; }

        public string Product { get; set; }

        public string Name { get; set; }

        public string Features { get; set; }

        public string Usb { get; set; }

        public string TransportId { get; set; }

        public string Message { get; set; }
      
        public bool IsOnline
        {
            get
            {
                var status = false;
                if (State == DeviceState.Online)
                    status = true;
                return status;
            }
        }
        public bool IsShowWarning
        {
            get
            {
                var status = true;
                if (State == DeviceState.Online)
                    status = false;
                return status;
            }
        }
        public static DeviceData CreateFromAdbData(string data)
        {
            Match match = Regex.Match(data);
            if (match.Success)
            {
                return new DeviceData
                {
                    Serial = match.Groups["serial"].Value,
                    State = GetStateFromString(match.Groups["state"].Value),
                    Model = match.Groups["model"].Value,
                    Product = match.Groups["product"].Value,
                    Name = match.Groups["device"].Value,
                    Features = match.Groups["features"].Value,
                    Usb = match.Groups["usb"].Value,
                    TransportId = match.Groups["transport_id"].Value,
                    Message = match.Groups["message"].Value
                };
            }

            throw new ArgumentException("Invalid device list data '" + data + "'");
        }

        public override string ToString()
        {
            return Serial;
        }

        internal static DeviceState GetStateFromString(string state)
        {
            DeviceState result = DeviceState.Unknown;
            if (string.Equals(state, "device", StringComparison.OrdinalIgnoreCase))
            {
                result = DeviceState.Online;
              
            }
            else if (string.Equals(state, "no permissions", StringComparison.OrdinalIgnoreCase))
            {
                result = DeviceState.NoPermissions;
            }
            else if (!Enum.TryParse<DeviceState>(state, ignoreCase: true, out result))
            {
                result = DeviceState.Unknown;
            }

            return result;
        }
    }
}