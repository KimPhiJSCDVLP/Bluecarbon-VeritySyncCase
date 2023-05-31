using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeritySyncCase.Converter
{
    class BackgroundValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                int index = 0;
                foreach (var itm in BindableLayout.GetItemsSource((parameter as StackLayout)))
                {
                    if (itm != value)
                    {
                        index++;
                    }
                    else
                    {
                        return (index % 2 == 0) ? Color.FromHex("#BACADE") : Color.FromHex("#D4DDE9");
                    }
                }
            }
            return Color.FromHex("#BACADE");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
