using System.Collections;
using System.Globalization;

namespace MauiAppCollectionWithRowNumber
{
    public class RowNumberConverter : BindableObject, IValueConverter
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            nameof(ItemsSource), typeof(IEnumerable), typeof(RowNumberConverter), defaultValue: null, defaultBindingMode: BindingMode.OneWay);

        public IEnumerable ItemsSource { get => (IEnumerable)GetValue(ItemsSourceProperty); set => SetValue(ItemsSourceProperty, value); }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ToRowNumber(rowData: value, rows: ItemsSource, format: parameter.ToString());
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        private string ToRowNumber(object rowData, IEnumerable rows, string format)
        {
            return (rows.Cast<object>().TakeWhile(x => !ReferenceEquals(x, rowData)).Count() + 1).ToString(format);
        }
    }
}
