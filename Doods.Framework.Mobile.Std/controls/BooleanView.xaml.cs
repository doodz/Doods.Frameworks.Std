using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Doods.Framework.Mobile.Std.controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BooleanView : ContentView
	{

		public static readonly BindableProperty ValueProperty =
			BindableProperty.Create(nameof(Value),
				typeof(bool), typeof(BooleanView), false,propertyChanged: ValuePropertyChanged);

		private static void ValuePropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
		{
			
		}

		public bool Value
		{
			get => (bool)GetValue(ValueProperty);
			set => SetValue(ValueProperty, value);
		}


		public static readonly BindableProperty TextProperty =
			BindableProperty.Create(nameof(Text), typeof(string), typeof(BooleanView));

		public string Text
		{
			get => (string)GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}

		public static readonly BindableProperty DescriptionProperty =
			BindableProperty.Create(nameof(Description), typeof(string), typeof(BooleanView));

		public string Description
		{
			get => (string)GetValue(DescriptionProperty);
			set => SetValue(DescriptionProperty, value);
		}



		public static readonly BindableProperty SubtitleStyleProperty = BindableProperty.Create(nameof(SubtitleStyleProperty),
			typeof(Style), typeof(BooleanView));


		public Style SubtitleStyle
		{
			get => (Style)GetValue(SubtitleStyleProperty);
			set => SetValue(SubtitleStyleProperty, (object)value);
		}


		public static readonly BindableProperty TextColorProperty =
			BindableProperty.Create(nameof(TextColor),
				typeof(Color), typeof(BooleanView), Color.Blue,
				propertyChanged: TextColorPropertyChanged);


		private static void TextColorPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
		{
			//if (newvalue == oldvalue)
			//    return;
			//var val = (Color)newvalue;
			//((BooleanViewCell)bindable)TextColor = val;
			//var oldVal = (Color)bindable.GetValue(ClearableEntryViewValidator.NormalColorProperty);
			//if (oldVal == val)
			//    return;

			//bindable.SetValue(ClearableEntryViewValidator.NormalColorProperty, val);
		}


		public Color TextColor
		{
			get => (Color)GetValue(TextColorProperty);
			set => SetValue(TextColorProperty, value);
		}
		public BooleanView()
		{
			InitializeComponent ();

			MySwitch.BindingContext = this;
			MySwitch.SetBinding(Entry.TextProperty,
				nameof(Value), BindingMode.TwoWay);
		}
	}
}