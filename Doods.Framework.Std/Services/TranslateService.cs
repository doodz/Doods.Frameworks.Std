using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mime;
using System.Text;

namespace Doods.Framework.Std.Services
{
    public class TranslateService : ITranslateService
    {
        private System.Resources.ResourceManager _resourceManager;
        public TranslateService(System.Resources.ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
            
           
        }

        public string Translate(string resourceName)
        {
            return Translate(resourceName, _resourceManager);
        }


        public string Translate(string resourceName, System.Resources.ResourceManager resourceManager)
        {
            return Translate(resourceName, resourceManager, CultureInfo.CurrentCulture);

        }

        public string Translate(string resourceName, System.Resources.ResourceManager resourceManager,CultureInfo culture)
        {
            var translation = resourceManager.GetString(resourceName, culture);

            //var translation = ResMgr.Value.GetString(Text, ci);
            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException(
                    $"Key '{resourceName}' was not found in resources '{ resourceManager.GetType().Assembly}' for culture '{culture.Name}'.");
#else
                translation = resourceName; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
            }
            return translation;
        }
    }
}
