using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Resources;
using Doods.Framework.Std;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Doods.Framework.Mobile.Std.Controls
{
    public class EnumBindableDescriptionLocalizablePicker<T> : Picker where T : struct
    {
        public EnumBindableDescriptionLocalizablePicker()
        {
            SelectedIndexChanged += OnSelectedIndexChanged;
            LoadList();
        }

        private void LoadList()
        {
            Items.Clear();
            foreach (var value in System.Enum.GetValues(typeof(T))) Items.Add(GetEnumDescription(value));
            //SelectedItem = default(T);
            SelectedIndex = 0;
        }

        public static readonly BindableProperty DescriptionsHasPrecedenceProperty =
            BindableProperty.Create(nameof(DescriptionHasPrecedence), typeof(bool), typeof(EnumBindableDescriptionLocalizablePicker<T>),
                false, propertyChanged: (bindable, value, newValue) =>
                {
                    ((EnumBindableDescriptionLocalizablePicker<T>)bindable).LoadList();
                });
         
        public static readonly BindableProperty TranslateProperty =
            BindableProperty.Create(nameof(Translate), typeof(ITranslateService), typeof(EnumBindableDescriptionLocalizablePicker<T>),
                 propertyChanged: (bindable, value, newValue) =>
                {
                    ((EnumBindableDescriptionLocalizablePicker<T>)bindable).LoadList();
                });


        public ITranslateService Translate
        {
            get => (ITranslateService)GetValue(TranslateProperty);
            set => SetValue(TranslateProperty, value);
        }
        public bool DescriptionHasPrecedence
        {
            get => (bool)GetValue(DescriptionsHasPrecedenceProperty);
            set => SetValue(DescriptionsHasPrecedenceProperty, value);
        }

        public new static BindableProperty SelectedItemProperty =
            BindableProperty.Create(nameof(SelectedItem), typeof(T), typeof(EnumBindableDescriptionLocalizablePicker<T>),
                default(T), propertyChanged: OnSelectedItemChanged, defaultBindingMode: BindingMode.TwoWay);

        public new T SelectedItem
        {
            get => (T)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        private void OnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            if (SelectedIndex < 0 || SelectedIndex > Items.Count - 1)
            {
                SelectedItem = default(T);
                return;
            }
            if (!System.Enum.TryParse(Items[SelectedIndex], out T match))
            {
                match = GetEnumByDescription(Items[SelectedIndex]);
            }
            SelectedItem = (T)System.Enum.Parse(typeof(T), match.ToString());
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (newvalue == null) return;
            if (bindable is EnumBindableDescriptionLocalizablePicker<T> picker) picker.SelectedIndex =
                picker.Items.IndexOf(newvalue.ToString());
        }


        private string GetEnumDescription(object value)
        {
            var result = value.ToString();
            var attribute = typeof(T).GetRuntimeField(value.ToString()).GetCustomAttributes<DescriptionAttribute>(false)
                .SingleOrDefault();
            if (attribute != null && (bool)GetValue(DescriptionsHasPrecedenceProperty)) return attribute.Description;
            var match = Translate?.Translate($"{typeof(T).Name}_{value}");
            return !string.IsNullOrWhiteSpace(match) ? match : result;
        }
        private T GetEnumByDescription(string description)
        {
            return System.Enum.GetValues(typeof(T)).Cast<T>().FirstOrDefault(x => string.Equals(GetEnumDescription(x), description));
        }
    }
}