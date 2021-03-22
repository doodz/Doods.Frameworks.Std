using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Doods.Framework.Mobile.Std.controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClearableEntryView : ContentView
    {
        public static readonly BindableProperty ClearCmdProperty = BindableProperty.Create(nameof(ClearCmd),
            typeof(ICommand),
            typeof(ClearableEntryView), defaultBindingMode: BindingMode.OneWay,
            propertyChanged: ClearCmdPropertyChanged);

        public static readonly BindableProperty NormalColorProperty =
            BindableProperty.Create(nameof(NormalColor),
                typeof(Color), typeof(ClearableEntryView), Color.Blue, propertyChanged: NormalColorPropertyChanged);

        public static readonly BindableProperty EntryStyleProperty = BindableProperty.Create(nameof(EntryStyle),
            typeof(Style), typeof(ClearableEntryView));

        public static readonly BindableProperty EntryTextProperty =
            BindableProperty.Create(nameof(EntryText), typeof(string), typeof(ClearableEntryView),
                propertyChanged: EntryTextPropertyChanged, defaultValue: default(string));

        public static readonly BindableProperty LabelTextProperty =
            BindableProperty.Create(nameof(LabelText), typeof(string), typeof(ClearableEntryView));

        public ClearableEntryView()
        {
            //this.BindingContext = this;
            InitializeComponent();
            //MyFloatingActionButton.Command = ClearCmd2;

            MyEntry.BindingContext = this;
            MyEntry.SetBinding(Entry.TextProperty,
                nameof(EntryText), BindingMode.TwoWay);

            MyLabel.BindingContext = this;
            MyLabel.SetBinding(Label.TextProperty,
                nameof(LabelText), BindingMode.OneWay);

            //MyEntry.SetBinding( Entry.TextProperty,EntryText);
        }

        public ICommand ClearCmd
        {
            get => (ICommand) GetValue(ClearCmdProperty);
            set => SetValue(ClearCmdProperty, value);
        }

        public ICommand ClearCmd2 => new Command(Clear);


        public Color NormalColor
        {
            get => (Color) GetValue(NormalColorProperty);
            set => SetValue(NormalColorProperty, value);
        }


        public Style EntryStyle
        {
            get => (Style) GetValue(StyleProperty);
            set => SetValue(StyleProperty, value);
        }

        public string EntryText
        {
            get => (string) GetValue(EntryTextProperty);
            set => SetValue(EntryTextProperty, value);
        }

        public string LabelText
        {
            get => (string) GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        private static void ClearCmdPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (newvalue == oldvalue)
                return;
            var val = (ICommand) newvalue;
            ((ClearableEntryView) bindable).MyFloatingActionButton.Command = val;
            //var oldVal = (Color)bindable.GetValue(ClearableEntryView.NormalColorProperty);
            //if (oldVal == val)
            //    return;

            //bindable.SetValue(ClearableEntryView.NormalColorProperty, val);
        }

        private void Clear()
        {
            EntryText = null;
        }


        private static void NormalColorPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (newvalue == oldvalue)
                return;
            var val = (Color) newvalue;
            ((ClearableEntryView) bindable).MyFloatingActionButton.NormalColor = val;
            //var oldVal = (Color)bindable.GetValue(ClearableEntryView.NormalColorProperty);
            //if (oldVal == val)
            //    return;

            //bindable.SetValue(ClearableEntryView.NormalColorProperty, val);
        }

        private static void EntryTextPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var entry = ((ClearableEntryView) bindable).MyEntry;
            // ISSUE: reference to a compiler-generated field


            if (newvalue == oldvalue)
                return;
            var val = (string) newvalue;


            //((ClearableEntryView)bindable).MyEntry.Text = val;
            //var oldVal = (Color)bindable.GetValue(ClearableEntryView.NormalColorProperty);
            //if (oldVal == val)
            //    return;

            //bindable.SetValue(ClearableEntryView.NormalColorProperty, val);
        }
    }
}