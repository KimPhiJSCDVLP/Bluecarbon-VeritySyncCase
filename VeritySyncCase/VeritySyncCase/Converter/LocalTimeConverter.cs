using System.Globalization;

namespace VeritySyncCase.Converter
{
    public class LocalTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;
            var date = (DateTime)value;
            return date.ToLocalTime().ToString("hh:mm tt", CultureInfo.InvariantCulture);
        }        

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        } 
    }
}
