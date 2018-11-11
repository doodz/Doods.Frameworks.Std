using Doods.Framework.Mobile.Std.Converters;

namespace Doods.Framework.Mobile.Std.Enum
{
    /// <summary>
    /// Class who use type-safe enum pattern
    /// </summary>
    public sealed class SvgIconTarget
    {
        private static readonly ImageEnumEmbeddedResourceConverter ImageEnumEmbeddedResourceConverter = new ImageEnumEmbeddedResourceConverter();
        public static readonly SvgIconTarget AddBox = new SvgIconTarget(nameof(AddBox), "ic_add_box_24px.svg");
        public static readonly SvgIconTarget AddCircle = new SvgIconTarget(nameof(AddCircle), "ic_add_circle_24px.svg");

        public static readonly SvgIconTarget Info = new SvgIconTarget(nameof(Info), "ic_info_24px.svg");
        public static readonly SvgIconTarget InfoOutline = new SvgIconTarget(nameof(InfoOutline), "ic_info_outline_24px.svg");
        public static readonly SvgIconTarget ChevronRight = new SvgIconTarget(nameof(ChevronRight), "ic_chevron_right_24px.svg");
        public static readonly SvgIconTarget Computer = new SvgIconTarget(nameof(Computer), "ic_computer_24px.svg");

        public readonly string IconName;
        public readonly string IconFile;
       
        public  string ResourceFile { get; }
        private SvgIconTarget(string iconName,string iconFile)
        {
            IconName = iconName;
            IconFile = iconFile;
            
           
            ResourceFile = (string)ImageEnumEmbeddedResourceConverter.Convert(iconFile, null, null, null);  
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
