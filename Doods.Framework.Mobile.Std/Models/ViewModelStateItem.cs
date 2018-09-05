using Doods.Framework.Mobile.Std.Interfaces;
using Xamarin.Forms;

namespace Doods.Framework.Mobile.Std.Models
{
    public class ViewModelStateItem : BaseItem
    {
        public Command ShowCurrentCmd { get; private set; }
        public IViewModel ViewModel { get; }

        public ViewModelStateItem(IViewModel viewModel)
        {
            ViewModel = viewModel;
            ShowCurrentCmd = ViewModel.CmdState;
        }

        private bool _isRunning;
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }
    }
}