using System;
using Xamarin.Forms;

namespace Doods.Framework.Mobile.Std.Controls
{
    /// <summary>
    ///     Control template for Title only.
    /// </summary>
    public class TitleTemplate : ContentView
    {

        public Label titleLabel;
        public Label subTitleLabel;

        public TitleTemplate()
        {
            titleLabel = new Label();
            titleLabel.SetBinding(Label.TextProperty, new TemplateBinding(nameof(TitledFrameView.Title)));
            titleLabel.SetBinding(StyleProperty, new TemplateBinding(nameof(TitledFrameView.TitleStyle)));
            titleLabel.HorizontalOptions = LayoutOptions.FillAndExpand;

            subTitleLabel = new Label();
            subTitleLabel.SetBinding(Label.TextProperty, new TemplateBinding(nameof(TitledFrameView.SubTitle)));
            subTitleLabel.SetBinding(StyleProperty, new TemplateBinding(nameof(TitledFrameView.SubTitleStyle)));
            subTitleLabel.HorizontalOptions = LayoutOptions.FillAndExpand;
            Content = new StackLayout
            {
                
                Children =
                {
                    new StackLayout
                    {
                        Padding = 4,
                        BackgroundColor = Color.CornflowerBlue,
                        Orientation = StackOrientation.Horizontal,
                        Spacing = 10,
                        Children = {titleLabel, subTitleLabel}
                    },
                    new ContentPresenter()
                }
            };
         this.BindingContextChanged+=OnBindingContextChanged;   
        }

        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            
        }
    }
}