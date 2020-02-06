using System;
using System.Globalization;
using Doods.Framework.Mobile.Std.Enum;
using Xamarin.Forms;

namespace Doods.Framework.Mobile.Std.Converters
{
    public class BoolToCheckSvgResourceFileonverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
            {
                return b ? SvgIconTarget.Checked.ResourceFile : SvgIconTarget.Unchecked.ResourceFile;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

   
}