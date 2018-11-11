using System;
using Doods.Framework.Std.Validation;
using System.Windows.Input;
using Xamarin.Forms;

namespace Doods.Framework.Mobile.Std.Validation
{
    public class ValidatableObjectView<T> : ValidatableObject<T>
    {
        private string _title;
        private Keyboard _keyboard = Keyboard.Default;
        public ICommand ValidateCommand => new Command(() => Validate());

        public string Title
        {
            get => _title;
            private set => SetProperty(ref _title, value);
        }

        public Keyboard Keyboard
        {
            get => _keyboard;
            private set => SetProperty(ref _keyboard, value);
        } 
         
        public ValidatableObjectView(string title, bool autoValidation, Keyboard keyboard) : this(title, autoValidation)
        {
            if (keyboard == null)
                throw new ArgumentNullException(nameof(keyboard));
            Keyboard = keyboard;
        }

        public ValidatableObjectView(string title ,bool autoValidation) : base(autoValidation)
        {
            Title = title;
        }
    }
}