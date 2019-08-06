using System.Xml;

namespace Doods.Framework.Std
{
    public interface IConfiguration
    {
        string MobileCenterKey { get; }

        string HockeyAppKey { get; }
        string NumeroVersion { get; }

        string AdsKey { get; }
        string RewardedVideoKey { get; }
        void LoadConfiguration(XmlReader xmlContent);
    }
}