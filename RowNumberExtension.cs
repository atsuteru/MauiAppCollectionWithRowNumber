using System.Collections;
using System.Runtime.CompilerServices;

namespace MauiAppCollectionWithRowNumber
{
    public class RowNumberExtension : BindableObject, IMarkupExtension<BindingBase>
    {
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(RowNumberExtension), null, BindingMode.OneWay);
        public static readonly BindableProperty ItemProperty =
            BindableProperty.Create(nameof(Item), typeof(object), typeof(RowNumberExtension), null, BindingMode.OneWay);
        public static readonly BindableProperty FormatProperty =
            BindableProperty.Create(nameof(Format), typeof(string), typeof(RowNumberExtension), null, BindingMode.OneWay);
        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create(nameof(Value), typeof(string), typeof(RowNumberExtension), string.Empty, BindingMode.OneWayToSource);

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }
        public object Item
        {
            get => (object)GetValue(ItemProperty);
            set => SetValue(ItemProperty, value);
        }
        public string Format
        {
            get => (string)GetValue(FormatProperty);
            set => SetValue(FormatProperty, value);
        }
        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        BindingBase IMarkupExtension<BindingBase>.ProvideValue(IServiceProvider serviceProvider)
        {
            return (BindingBase)((IMarkupExtension)this).ProvideValue(serviceProvider);
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            ProvideContext(serviceProvider);
            return new Binding(ValueProperty.PropertyName, source: this);
        }
        private void ProvideContext(IServiceProvider serviceProvider)
        {
            var target = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            var targetObject = (BindableObject)target.TargetObject;
            EventHandler bindingChanged = null;
            bindingChanged = (sender, e) =>
            {
                BindingContext = targetObject.BindingContext;
                targetObject.BindingContextChanged -= bindingChanged;
            };
            targetObject.BindingContextChanged += bindingChanged;
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            switch (propertyName)
            {
                case nameof(ItemsSource):
                case nameof(Item):
                case nameof(Format):
                    Value = ToRowNumber(Item, ItemsSource, Format);
                    break;
                default:
                    break;
            }
            base.OnPropertyChanged(propertyName);
        }

        protected virtual string ToRowNumber(object rowData, IEnumerable rows, string format)
        {
            if (rows == null || rowData == null)
            {
                return string.Empty;
            }
            return (rows.Cast<object>().TakeWhile(x => !ReferenceEquals(x, rowData)).Count() + 1).ToString(format ?? "#");
        }
    }
}
