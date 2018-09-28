using Doods.Framework.Mobile.Std.Enum;
using Doods.Framework.Mobile.Std.Interfaces;
using System.Windows.Input;
using Xamarin.Forms;

namespace Doods.Framework.Mobile.Std.Models
{
    public class ViewModelStateItem : BaseItem
    {
        public ICommand ShowCurrentCmd { get; private set; }
        private ICommand _addCmd;

        public ICommand AddCmd
        {
            get => _addCmd;
            set => SetProperty(ref _addCmd, value);
        }

        public IViewModel ViewModel { get; }

        public ViewModelStateItem(IViewModel viewModel)
        {
            ViewModel = viewModel;
            ShowCurrentCmd = ViewModel.CmdState;
            Icon = SvgIconTarget.Info;
        }

       
        private SvgIconTarget _icon;

        public SvgIconTarget Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }


        private Color? _color;

        public Color? Color
        {
            get => _color;
            set => SetProperty(ref _color, value);
        }

        private bool _isRunning;

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        private bool _canAdd;

        public bool CanAdd
        {
            get => _canAdd;
            set => SetProperty(ref _canAdd, value);
        }
    }
}