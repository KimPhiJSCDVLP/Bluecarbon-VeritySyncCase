using System.Globalization;

namespace VeritySyncCase.Converter
{
    public class GreaterToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long p = 0;
            var pCheck = long.TryParse(parameter.ToString(), out p);
            long v = 0;
            var vCheck = long.TryParse(value.ToString(), out v);
            return v > p;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return parameter;

            //it's false, so don't bind it back
            throw new Exception("EqualityToBooleanConverter: It's false, I won't bind back.");
        }
    }
}
