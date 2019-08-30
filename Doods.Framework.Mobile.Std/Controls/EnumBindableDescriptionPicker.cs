using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace Doods.Framework.Mobile.Std.Controls
{
    public class EnumBindableDescriptionPicker<T> : Picker where T : struct
    {
        public new static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create(nameof(SelectedItem), typeof(T), typeof(EnumBindableDescriptionPicker<T>),
                default(T), propertyChanged: OnSelectedItemChanged, defaultBindingMode: BindingMode.TwoWay);

        public EnumBindableDescriptionPicker()
        {
            SelectedIndexChanged += OnSelectedIndexChanged;
            foreach (var value in System.Enum.GetValues(typeof(T))) Items.Add(GetEnumDescription(value));
        }

        public new T SelectedItem
        {
            get => (T) GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        private void OnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            if (SelectedIndex < 0 || SelectedIndex > Items.Count - 1)
            {
                SelectedItem = default;
                return;
            }

            if (!System.Enum.TryParse(Items[SelectedIndex], out T match))
                match = GetEnumByDescription(Items[SelectedIndex]);

            SelectedItem = (T) System.Enum.Parse(typeof(T), match.ToString());
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (newvalue == null) return;
            if (bindable is EnumBindableDescriptionPicker<T> picker)
                picker.SelectedIndex =
                    picker.Items.IndexOf(newvalue.ToString());
        }


        private string GetEnumDescription(object value)
        {
            var result = value.ToString();
            var attribute = typeof(T).GetRuntimeField(value.ToString()).GetCustomAttributes<DescriptionAttribute>(false)
                .SingleOrDefault();
            return attribute != null ? attribute.Description : result;
        }

        private T GetEnumByDescription(string description)
        {
            return System.Enum.GetValues(typeof(T)).Cast<T>()
                .FirstOrDefault(x => string.Equals(GetEnumDescription(x), description));
        }
    }
}