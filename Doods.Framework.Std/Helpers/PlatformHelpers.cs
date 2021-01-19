using System.Runtime.InteropServices;

namespace Doods.Framework.Std.Helpers
{
   

    public static class PlatformHelpers
    {
        
        public static bool IsWindows() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
       public static bool IsLinux() => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        public static bool IsOsx() => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
    }
}
