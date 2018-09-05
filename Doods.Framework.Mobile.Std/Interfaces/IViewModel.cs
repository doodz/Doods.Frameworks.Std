using Doods.Framework.Mobile.Std.Mvvm;
using Xamarin.Forms;

namespace Doods.Framework.Mobile.Std.Interfaces
{
    public interface IViewModel
    {
        Command CmdState { get; }
        ViewModelState ViewModelState { get; }
    }
}