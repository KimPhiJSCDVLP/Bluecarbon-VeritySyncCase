using System.Globalization;

namespace VeritySyncCase.Converter
{
    public class PrefixConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var title = parameter?.ToString();
            var data = value?.ToString();
            if (!string.IsNullOrEmpty(data))
            {
                return $"{title} {data}";
            }
            return title;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
