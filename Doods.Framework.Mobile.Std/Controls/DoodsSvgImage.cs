
using Doods.Framework.Mobile.Std.Enum;
using FFImageLoading.Svg.Forms;

namespace Doods.Framework.Mobile.Std.Controls
{
    public class DoodsSvgImage : SvgCachedImage
    {
        //todo add statut => error ok enable ...


        public DoodsSvgImage()
        {
            ReplaceStringMap = SvgIconTarget.ReplaceColor;
        }
    }
}