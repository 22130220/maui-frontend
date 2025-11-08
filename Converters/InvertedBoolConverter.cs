using System.Globalization;

namespace MauiFrontend.Converters
{
    public class InvertedBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
                return !boolValue;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
                return !boolValue;
            return false;
        }
    }

    public class AvatarSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return "default_avatar.png";
            }

            string avatarPath = value.ToString();

            if (avatarPath.StartsWith("http://") || avatarPath.StartsWith("https://"))
            {
                return ImageSource.FromUri(new Uri(avatarPath));
            }

            return avatarPath;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StarFilterColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int selectedStar && parameter is string paramStar)
            {
                int filterValue = int.Parse(paramStar);
                return selectedStar == filterValue ? Color.FromArgb("#FF9800") : Color.FromArgb("#999999");
            }
            return Color.FromArgb("#999999");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SortOrderColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string selectedOrder && parameter is string buttonOrder)
            {
                // So sánh order đã chọn với button order
                return selectedOrder == buttonOrder
                    ? Color.FromArgb("#FF9800")  // Màu cam khi được chọn
                    : Color.FromArgb("#999999"); // Màu xám khi không chọn
            }
            return Color.FromArgb("#999999");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class InvertedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool b && !b;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DateTimeFormatConverter : IValueConverter
    {

        public string Format { get; set; } = "dd/MM/yyyy";
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string dateStr && DateTime.TryParse(dateStr, null, DateTimeStyles.RoundtripKind, out var dt))
                return dt.ToString(Format, CultureInfo.CurrentCulture);
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}