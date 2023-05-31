using System;
using System.Globalization;

namespace VeritySyncCase.Converter
{
    public class HeightValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && (double)value > 0)
            {
                return value;
            }
            int height = 0;
            if(int.TryParse(parameter.ToString(), out height))
                return height;
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
