using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Doods.Framework.Mobile.Std.controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ValidatableObjectControl : ContentView
    {
        public static readonly BindableProperty NextEntryProperty =
            BindableProperty.Create(nameof(NextEntry), typeof(View), typeof(Entry));

        public View NextEntry
        {
            get => (View)GetValue(NextEntryProperty);
            set => SetValue(NextEntryProperty, value);
        }

        public ValidatableObjectControl()
        {
            InitializeComponent();
           
        }

        public void OnNext()
        {
           
            NextEntry?.Focus();
        }
    }
}