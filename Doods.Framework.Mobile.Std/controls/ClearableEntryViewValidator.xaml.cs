using Doods.Framework.Std.Validation;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Doods.Framework.Mobile.Std.controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClearableEntryViewValidator : ContentView
    {
        public static readonly BindableProperty ClearCmdProperty = BindableProperty.Create(nameof(ClearCmd),
            typeof(ICommand),
            typeof(ClearableEntryViewValidator), defaultBindingMode: BindingMode.OneWay,
            propertyChanged: ClearCmdPropertyChanged);

        private static void ClearCmdPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (newvalue == oldvalue)
                return;
            var val = (ICommand) newvalue;
            ((ClearableEntryViewValidator) bindable).MyFloatingActionButton.Command = val;
            //var oldVal = (Color)bindable.GetValue(ClearableEntryViewValidator.NormalColorProperty);
            //if (oldVal == val)
            //    return;

            //bindable.SetValue(ClearableEntryViewValidator.NormalColorProperty, val);
        }

        public ICommand ClearCmd
        {
            get => (ICommand) GetValue(ClearCmdProperty);
            set => SetValue(ClearCmdProperty, value);
        }

        public ICommand ClearCmd2 => new Command(Clear);

        private void Clear()
        {
            EntryText = null;
        }

        public static readonly BindableProperty NormalColorProperty =
            BindableProperty.Create(nameof(NormalColor),
                typeof(Color), typeof(ClearableEntryViewValidator), Color.Blue,
                propertyChanged: NormalColorPropertyChanged);


        private static void NormalColorPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (newvalue == oldvalue)
                return;
            var val = (Color) newvalue;
            ((ClearableEntryViewValidator) bindable).MyFloatingActionButton.NormalColor = val;
            //var oldVal = (Color)bindable.GetValue(ClearableEntryViewValidator.NormalColorProperty);
            //if (oldVal == val)
            //    return;

            //bindable.SetValue(ClearableEntryViewValidator.NormalColorProperty, val);
        }


        public Color NormalColor
        {
            get => (Color) GetValue(NormalColorProperty);
            set => SetValue(NormalColorProperty, value);
        }

        public static readonly BindableProperty EntryStyleProperty = BindableProperty.Create(nameof(EntryStyle),
            typeof(Style), typeof(ClearableEntryViewValidator));


        public Style EntryStyle
        {
            get => (Style) GetValue(StyleProperty);
            set => SetValue(StyleProperty, (object) value);
        }


        public static readonly BindableProperty ValidateOnLostFocusProperty = BindableProperty.Create(nameof(ValidateOnLostFocus),
            typeof(bool), typeof(ClearableEntryViewValidator),true);


        public bool ValidateOnLostFocus
        {
            get => (bool)GetValue(ValidateOnLostFocusProperty);
            set => SetValue(ValidateOnLostFocusProperty, (object)value);
        }

        public static readonly BindableProperty EntryTextProperty =
            BindableProperty.Create(nameof(EntryText), typeof(ValidatableObject<string>),
                typeof(ClearableEntryViewValidator), propertyChanged: EntryTextPropertyChanged,
                defaultValue: default(string));

        private static void EntryTextPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var entry = ((ClearableEntryViewValidator) bindable).MyEntry;
            // ISSUE: reference to a compiler-generated field


            if (newvalue == oldvalue)
                return;
            var val = (ValidatableObject<string>) newvalue;


            //((ClearableEntryViewValidator)bindable).MyEntry.Text = val;
            //var oldVal = (Color)bindable.GetValue(ClearableEntryViewValidator.NormalColorProperty);
            //if (oldVal == val)
            //    return;

            //bindable.SetValue(ClearableEntryViewValidator.NormalColorProperty, val);
        }

        public ValidatableObject<string> EntryText
        {
            get => (ValidatableObject<string>) GetValue(EntryTextProperty);
            set => SetValue(EntryTextProperty, value);
        }

        public static readonly BindableProperty LabelTextProperty =
            BindableProperty.Create(nameof(LabelText), typeof(string), typeof(ClearableEntryViewValidator));

        public string LabelText
        {
            get => (string) GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        public ClearableEntryViewValidator()
        {
            //this.BindingContext = this;
            InitializeComponent();
            //MyFloatingActionButton.Command = ClearCmd2;

            MyEntry.BindingContext = this;
            MyEntry.SetBinding(Entry.TextProperty,
                "EntryText.Value", BindingMode.TwoWay);

            MyEntry.Unfocused += MyEntry_Unfocused;

            MyLabel.BindingContext = this;
            MyLabel.SetBinding(Label.TextProperty,
                nameof(LabelText), BindingMode.OneWay);

            MyLabelError.BindingContext = this;
            MyLabelError.SetBinding(Label.TextProperty,
                "EntryText.FirstError", BindingMode.OneWay);
        }

        private void MyEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if(ValidateOnLostFocus)
            EntryText?.Validate();
        }
    }
}