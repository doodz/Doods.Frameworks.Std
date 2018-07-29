using Doods.Framework.Mobile.Std.Enum;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Doods.Framework.Mobile.Std.controls
{
    public class FloatingActionButton : View
    {
        public static readonly BindableProperty SizeProperty =
            BindableProperty.Create<FloatingActionButton, FloatingActionButtonSize>(mn => mn.Size,
                FloatingActionButtonSize.Normal);

        public static readonly BindableProperty NormalColorProperty =
            BindableProperty.Create<FloatingActionButton, Color>(mn => mn.NormalColor, Color.Blue);

        public static readonly BindableProperty RippleColorProperty =
            BindableProperty.Create<FloatingActionButton, Color>(mn => mn.RippleColor, Color.Gray);

        public static readonly BindableProperty DisabledColorProperty =
            BindableProperty.Create<FloatingActionButton, Color>(mn => mn.DisabledColor, Color.Gray);

        public static readonly BindableProperty HasShadowProperty =
            BindableProperty.Create<FloatingActionButton, bool>(mn => mn.HasShadow, false);

        public static readonly BindableProperty SourceProperty =
            BindableProperty.Create<FloatingActionButton, ImageSource>(mn => mn.Source, null);

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create<FloatingActionButton, ICommand>(mn => mn.Command, null,
                propertyChanged: HandleCommandChanged);

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create<FloatingActionButton, object>(mn => mn.CommandParameter, null);

        public static readonly BindableProperty AnimateOnSelectionProperty =
            BindableProperty.Create<FloatingActionButton, bool>(mn => mn.AnimateOnSelection, true);

        public FloatingActionButtonSize Size
        {
            get => (FloatingActionButtonSize)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public Color NormalColor
        {
            get => (Color)GetValue(NormalColorProperty);
            set => SetValue(NormalColorProperty, value);
        }

        public Color RippleColor
        {
            get => (Color)GetValue(RippleColorProperty);
            set => SetValue(RippleColorProperty, value);
        }

        public Color DisabledColor
        {
            get => (Color)GetValue(DisabledColorProperty);
            set => SetValue(DisabledColorProperty, value);
        }

        public bool HasShadow
        {
            get => (bool)GetValue(HasShadowProperty);
            set => SetValue(HasShadowProperty, value);
        }

        [TypeConverter(typeof(ImageSourceConverter))]
        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public bool AnimateOnSelection
        {
            get => (bool)GetValue(AnimateOnSelectionProperty);
            set => SetValue(AnimateOnSelectionProperty, value);
        }

        public event EventHandler<EventArgs> Clicked;

        public virtual void SendClicked()
        {
            var param = CommandParameter;

            if (Command != null && Command.CanExecute(param))
                Command.Execute(param);

            Clicked?.Invoke(this, EventArgs.Empty);
        }

        private void InternalHandleCommand(ICommand oldValue, ICommand newValue)
        {
            // TOOD: attach to CanExecuteChanged
        }

        private static void HandleCommandChanged(BindableObject bindable, ICommand oldValue, ICommand newValue)
        {
            (bindable as FloatingActionButton)?.InternalHandleCommand(oldValue, newValue);
        }
    }

}
