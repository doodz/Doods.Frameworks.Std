using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Doods.Framework.Mobile.Std.controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PopupListViewPage : PopupPage
	{
		public PopupListViewPage ()
		{
			InitializeComponent();
			listView.ItemsSource = new List<string>
			{
				"Test ListView",
				"Test ListView",
				"Test ListView",
				"Test ListView",
				"Test ListView",
				"Test ListView",
				"Test ListView",
				"Test ListView",
				"Test ListView",
				"Test ListView",
				"Test ListView",
				"Test ListView",
				"Test ListView",
				"Test ListView",
				"Test ListView",
				"Test ListView",
				"Test ListView",
				"Test ListView",
				"Test ListView",
				"Test ListView",
				"Test ListView",
				"Test ListView",
				"Test ListView",
				"Test ListView",
				"Test ListView"
			};
		}
		private async void OnClose(object sender, EventArgs e)
		{
			await PopupNavigation.Instance.PopAsync();
		}
	}
}