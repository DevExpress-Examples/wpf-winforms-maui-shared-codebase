using DataModel;
using DevExpress.Maui.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.Converters
{
    public class IsFilterEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is FilterValueInfo filterInfo && filterInfo != null)
            {
                return filterInfo.Count == 0;
            }
            else if (value is int selectedFilterItems)
            {
                return selectedFilterItems == 0;
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            AppTheme currentTheme = Application.Current.RequestedTheme;
            if (value is null)
            {
                return null;
            }
            OrderState state = (OrderState)value;
            var alpha = (float)parameter;
            Color res;
            if (currentTheme is AppTheme.Light)
            {
                res = state switch
                {
                    OrderState.Draft => Color.FromArgb("#60BA07"),
                    OrderState.Shipping => Color.FromArgb("#55ABDC"),
                    OrderState.Paid => Color.FromArgb("#3CAB76"),
                    OrderState.Processed => Color.FromArgb("#D46E60"),
                    _ => Color.FromArgb("#60BA07")
                };
            }
            else
            {
                res = state switch
                {
                    OrderState.Draft => Color.FromArgb("#A0CD73"),
                    OrderState.Shipping => Color.FromArgb("#8FC3E0"),
                    OrderState.Paid => Color.FromArgb("#67C596"),
                    OrderState.Processed => Color.FromArgb("#D88074"),
                    _ => Color.FromArgb("#A0CD73")
                };
            }

            return res.WithAlpha(alpha);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
