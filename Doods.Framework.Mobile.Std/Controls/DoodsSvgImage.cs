
using Doods.Framework.Mobile.Std.Enum;
using FFImageLoading.Svg.Forms;

namespace Doods.Framework.Mobile.Std.Controls
{
    public class DoodsSvgImage : SvgCachedImage
    {
        //todo add statut => error ok enable ...


        public DoodsSvgImage()
        {
            //Xamarin.Forms.Color redButtonStyle = (Xamarin.Forms.Color)this.Resources["MyColorButton"];
            
            ReplaceStringMap = SvgIconTarget.ReplaceColor;
        }
    }
}