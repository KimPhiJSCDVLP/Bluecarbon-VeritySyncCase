using CommunityToolkit.Mvvm.ComponentModel;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VeritySyncCase.Models
{
    public class DeviceDataDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        internal const string DeviceDataRegexString = "^(?<serial>[a-zA-Z0-9_-]+(?:\\s?[\\.a-zA-Z0-9_-]+)?(?:\\:\\d{1,})?)\\s+(?<state>device|connecting|offline|unknown|bootloader|recovery|download|authorizing|unauthorized|host|no permissions)(?<message>.*?)(\\s+usb:(?<usb>[^:]+))?(?:\\s+product:(?<product>[^:]+))?(\\s+model\\:(?<model>[\\S]+))?(\\s+device\\:(?<device>[\\S]+))?(\\s+features:(?<features>[^:]+))?(\\s+transport_id:(?<transport_id>[^:]+))?$";

        private static readonly Regex Regex = new Regex("^(?<serial>[a-zA-Z0-9_-]+(?:\\s?[\\.a-zA-Z0-9_-]+)?(?:\\:\\d{1,})?)\\s+(?<state>device|connecting|offline|unknown|bootloader|recovery|download|authorizing|unauthorized|host|no permissions)(?<message>.*?)(\\s+usb:(?<usb>[^:]+))?(?:\\s+product:(?<product>[^:]+))?(\\s+model\\:(?<model>[\\S]+))?(\\s+device\\:(?<device>[\\S]+))?(\\s+features:(?<features>[^:]+))?(\\s+transport_id:(?<transport_id>[^:]+))?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private string serial;
        public string Serial
        {
            get { return serial; }
            set { SetProperty(ref serial, value); }
        }

        private DeviceState state;
        public DeviceState State
        {
            get { return state; }
            set { SetProperty(ref state, value); }
        }

        private string model;
        public string Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }

        private string product;
        public string Product
        {
            get { return product; }
            set { SetProperty(ref product, value); }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private string features;
        public string Features
        {
            get { return features; }
            set { SetProperty(ref features, value); }
        }

        private string usb;
        public string Usb
        {
            get { return usb; }
            set { SetProperty(ref usb, value); }
        }

        private string transportId;
        public string TransportId
        {
            get { return transportId; }
            set { SetProperty(ref transportId, value); }
        }

        private string message;
        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }

        private bool isOnline;
        public bool IsOnline
        {
            get { return isOnline; }
            set { SetProperty(ref isOnline, value); }
        }

        private bool isShowWarning;
        public bool IsShowWarning
        {
            get { return isShowWarning; }
            set { SetProperty(ref isShowWarning, value); }
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