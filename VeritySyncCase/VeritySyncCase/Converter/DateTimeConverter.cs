using System.Globalization;

namespace VeritySyncCase.Converter
{
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;
            var date = (DateTime)value;
            if(parameter != null)
            {
                int pr = 0;
                if (int.TryParse(parameter.ToString(), out pr)) {
                    if(pr == 1)
                        return date.ToLocalTime().ToString("dd-MMM-yyyy HH:mm tt", CultureInfo.InvariantCulture);
                    else if(pr == 2)
                    {
                        return date.ToLocalTime().ToString("ddd dd MMM HH:mm", CultureInfo.InvariantCulture);
                    }
                    else if(pr == 3)
                    {
                        return date.ToLocalTime().ToString("dd MMM yyyy | HH:mm", CultureInfo.InvariantCulture);
                    }
                    else if (pr == 4) //date only
                    {
                        return date.ToLocalTime().ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
                    }
                    else if (pr == 5) //time only
                    {
                        return date.ToLocalTime().ToString("HH:mm", CultureInfo.InvariantCulture);
                    }
                }
            }
            return date.ToLocalTime().ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
