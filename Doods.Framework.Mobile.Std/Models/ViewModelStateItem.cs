using System;
using System.Windows.Input;
using Doods.Framework.Mobile.Std.Enum;
using Doods.Framework.Mobile.Std.Interfaces;
using Xamarin.Forms;

namespace Doods.Framework.Mobile.Std.Models
{

    //public enum StateLoadingPage
    //{
    //    Before,
    //    Loading,
    //    Okay,
    //    Fault,
    //    Warning
    //}

    public interface IStateItem
    {
        bool IsRunning { get; }

    }

    public class ViewModelStateItem : BaseItem, IStateItem
    {
        private Color? _color;
        private SvgIconTarget _icon;
        private bool _isRunning;
        private ICommand _showCurrentCmd;

        public ViewModelStateItem(IViewModel viewModel)
        {
            ViewModel = viewModel;
            ShowCurrentCmd = ViewModel.CmdState;
            Icon = SvgIconTarget.Info;
        }

        public ICommand ShowCurrentCmd
        {
            get => _showCurrentCmd;
            set => SetProperty(ref _showCurrentCmd, value);
        }

        public TResult RunFunc<TResult>(Func<TResult> myFunc)
        {
            _isRunning = true;
            var result =myFunc.Invoke();
            _isRunning = false;
            return result;
        }

        public void RunAction(Action myAction)
        {
            _isRunning = true;
            myAction.Invoke();
            _isRunning = false;

        }

        public IViewModel ViewModel { get; }

        public SvgIconTarget Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        public Color? Color
        {
            get => _color;
            set => SetProperty(ref _color, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }
    }
}