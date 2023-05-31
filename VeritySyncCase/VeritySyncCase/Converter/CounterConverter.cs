using System.Globalization;

namespace VeritySyncCase.Converter
{
    public class CounterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var title = parameter?.ToString();
            if (value != null)
            {
                var count = value.ToString();
                return $"{title} ({count})";
            }
            return title;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
