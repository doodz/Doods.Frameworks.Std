using Doods.Framework.Mobile.Std.Interfaces;

namespace Doods.Framework.Mobile.Std
{
    internal class MyDeviceInfo : IDeviceInfo
    {

        public  MyDeviceInfo()
        {
            var res = Plugin.DeviceInfo.CrossDeviceInfo.Current;


            Version =res.Version;
            Build =res.AppBuild;
           

        }
        public string Version { get; private set; }
        public string Build { get; private set; }
        public string ApplicationName => string.Empty;
        public string PackageName => string.Empty;
    }
}