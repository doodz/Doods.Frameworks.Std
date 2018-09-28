using Doods.Framework.Mobile.Std.Mvvm;
using System.Windows.Input;

namespace Doods.Framework.Mobile.Std.Interfaces
{
    public interface IViewModel
    {
        ICommand CmdState { get; }
        ViewModelState ViewModelState { get; }
        IColorPalette ColorPalette { get; }
    }
}