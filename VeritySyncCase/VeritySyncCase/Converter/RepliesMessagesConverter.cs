using System.Globalization;

namespace VeritySyncCase.Converter
{
    public class RepliesMessagesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                int num = 0;
                if (int.TryParse(value.ToString(), out num))
                {
                    return num > 1 ? $"{num} Replies" : $"{num} Reply";
                }
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
