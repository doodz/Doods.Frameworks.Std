using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Text;
using Doods.Framework.Std.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Doods.Framework.Mobile.Std.Resources
{
    [ContentProperty(nameof(Text))]
    public class TranslateExtension : TranslateService, IMarkupExtension
    {
     

       
        public string Text { get; set; }

        public TranslateExtension() : base(new ResourceManager("Doods.Framework.Mobile.Std.Resources.Resource", typeof(Resource).Assembly))
        {

        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return string.Empty;
            return this.Translate(Text);
        }
    }
}
