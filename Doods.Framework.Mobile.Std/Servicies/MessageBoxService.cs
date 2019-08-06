using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Doods.Framework.Mobile.Std.Interfaces;
using Xamarin.Forms;

namespace Doods.Framework.Mobile.Std.Servicies
{
    public class MessageBoxService : IMessageBoxService
    {
        private static Page CurrentMainPage { get { return Application.Current.MainPage; } }

        public async void ShowAlert(string title, string message, Action onClosed = null)
        {
            await CurrentMainPage.DisplayAlert(title, message, Resources.Resource.ButtonOK);
            onClosed?.Invoke();
        }

        public async Task<string> ShowActionSheet(string title, string cancel, string destruction = null, string[] buttons = null)
        {
            var displayButtons = buttons ?? new string[] { };
            var action = await CurrentMainPage.DisplayActionSheet(title, cancel, destruction, displayButtons);
            return action;
        }
    }
}
