using System;
using System.Collections.Generic;
using Doods.Framework.Mobile.Std.Converters;
using FFImageLoading.Forms;

namespace Doods.Framework.Mobile.Std.Enum
{
    /// <summary>
    /// Class who use type-safe enum pattern
    /// </summary>
    public sealed class SvgIconTarget
    {
        // blue app #96d1ff
        // red #ff0000
        // black #000000
        public static Dictionary<string, string> ReplaceColorToRed = new Dictionary<string, string> { { "#ff0000", "#FF3B30" } };
        public static Dictionary<string, string> ReplaceColorToGreen = new Dictionary<string, string> { { "#ff0000", "#34C759" } };

        /// <summary>
        /// Red to black 
        /// </summary>
        public static Dictionary<string, string> ReplaceColor = new Dictionary<string, string> {{ "#ff0000", "#000000" } };
        private static readonly ImageEnumEmbeddedResourceConverter ImageEnumEmbeddedResourceConverter = new ImageEnumEmbeddedResourceConverter();
        public static readonly SvgIconTarget AddBox = new SvgIconTarget(nameof(AddBox), "ic_add_box_24px.svg");
        public static readonly SvgIconTarget AddCircle = new SvgIconTarget(nameof(AddCircle), "ic_add_circle_24px.svg");

        public static readonly SvgIconTarget Info = new SvgIconTarget(nameof(Info), "ic_info_24px.svg");
        public static readonly SvgIconTarget InfoOutline = new SvgIconTarget(nameof(InfoOutline), "ic_info_outline_24px.svg");
        public static readonly SvgIconTarget ChevronRight = new SvgIconTarget(nameof(ChevronRight), "ic_chevron_right_24px.svg");
        public static readonly SvgIconTarget Computer = new SvgIconTarget(nameof(Computer), "ic_computer_24px.svg");

        public static readonly SvgIconTarget Delete = new SvgIconTarget(nameof(Delete), "ic_delete_24px.svg");
        public static readonly SvgIconTarget DeleteForever = new SvgIconTarget(nameof(DeleteForever), "ic_delete_forever_24px.svg");
        public static readonly SvgIconTarget ModeEdit = new SvgIconTarget(nameof(ModeEdit), "ic_mode_edit_24px.svg");

        public static readonly SvgIconTarget Checked = new SvgIconTarget(nameof(Checked), "ic_check_box_24px.svg");
        public static readonly SvgIconTarget Unchecked = new SvgIconTarget(nameof(Unchecked), "ic_check_box_outline_blank_24px.svg");
        public static readonly SvgIconTarget Done = new SvgIconTarget(nameof(Done), "ic_done_24px.svg");
        public static readonly SvgIconTarget Highlight = new SvgIconTarget(nameof(Highlight), "ic_highlight_off_24px.svg");

        public static readonly SvgIconTarget ErrorOutline = new SvgIconTarget(nameof(ErrorOutline), "ic_error_outline_24px.svg");
        public static readonly SvgIconTarget Error = new SvgIconTarget(nameof(Error), "ic_error_24px.svg");
        public static readonly SvgIconTarget CheckCircle = new SvgIconTarget(nameof(CheckCircle), "ic_check_circle_24px.svg");
        
        public static readonly SvgIconTarget Eject = new SvgIconTarget(nameof(Eject), "ic_eject_24px.svg");
        public static readonly SvgIconTarget PlayArrow = new SvgIconTarget(nameof(PlayArrow), "ic_play_arrow_24px.svg");
        public static readonly SvgIconTarget CloudDownload = new SvgIconTarget(nameof(CloudDownload), "ic_cloud_download_24px.svg");
        public static readonly SvgIconTarget Sync = new SvgIconTarget(nameof(Sync), "ic_sync_24px.svg");
        public static readonly SvgIconTarget Bullet = new SvgIconTarget(nameof(Bullet), "Bullet.svg");
        public readonly string IconName;
        public readonly string IconFile;
       
        public  string ResourceFile { get; }
        public EmbeddedResourceImageSource ImageSource { get; }
        public Xamarin.Forms.ImageSource ImageSource2 { get; }
        private SvgIconTarget(string iconName,string iconFile)
        {
            IconName = iconName;
            IconFile = iconFile;
            
           
            ResourceFile = (string)ImageEnumEmbeddedResourceConverter.Convert(iconFile, null, null, null);

            ImageSource = new EmbeddedResourceImageSource(new Uri(ResourceFile));
            ImageSource2 = Xamarin.Forms.Svg.SvgImageSource.FromSvgResource(ResourceFile);

        }


        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
       
        public string Source { get; set; }
        //{
        //    get
        //    {
        //        return (string)GetValue(SourceProperty);
        //    }
        //    set
        //    {
        //        SetValue(SourceProperty, value);
        //    }
        //}
        ///// <summary>
        ///// The source property.
        ///// </summary>
        //public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(string), typeof(SvgIconTarget), default(string), BindingMode.OneWay);

    }
}
