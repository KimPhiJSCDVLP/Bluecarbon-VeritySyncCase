using System.Globalization;

namespace VeritySyncCase.Converter
{
    public class TimeNoticeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = (DateTime)value;
            var dateNow = DateTime.UtcNow;
            if(date.Date == dateNow.Date)
            {
                if (date.Hour == dateNow.Hour && Math.Abs(date.Minute - dateNow.Minute) < 5)
                {
                    return "Just now";
                }
                else if (Math.Abs(date.Hour - dateNow.Hour) < 12)
                {
                    return date.ToLocalTime().ToString("h:mm tt", CultureInfo.InvariantCulture);
                }
                else if (Math.Abs(date.Hour - dateNow.Hour) > 12)
                {
                    var hoursDiff = Math.Abs(date.Hour - dateNow.Hour);
                    if (hoursDiff == 1)
                        return $"{hoursDiff} hour ago";
                    else
                        return $"{hoursDiff} hours ago";
                }
            }
            else
            {
                if (date.Year == dateNow.Year && date.Month == dateNow.Month && Math.Abs(date.Day - dateNow.Day) < 5)
                {
                    var dayDiff = Math.Abs(date.Day - dateNow.Day);
                    if (dayDiff == 1)
                        return $"{dayDiff} day ago";
                    else
                        return $"{dayDiff} days ago";
                }
                else
                {
                    return date.ToLocalTime().ToString("dd-MMM-yyyy h:mm tt", CultureInfo.InvariantCulture);
                }
            }
            return date.ToLocalTime().ToString("dd-MMM-yyyy h:mm tt", CultureInfo.InvariantCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
