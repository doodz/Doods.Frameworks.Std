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
          var translation = _resourceManager.GetString(resourceName);

            //var translation = ResMgr.Value.GetString(Text, ci);
            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException(
                    $"Key '{resourceName}' was not found in resources '{ _resourceManager.GetType().Assembly}' for culture '{CultureInfo.CurrentCulture.Name}'.");
#else
                translation = Text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
            }
            return translation;
        }
    }
}
