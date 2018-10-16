using Autofac;
using Doods.Framework.Mobile.Std.Interfaces;
using Doods.Framework.Mobile.Std.Servicies;
using Doods.Framework.Std;

namespace Doods.Framework.Mobile.Std.Config
{
    public class Bootstrapper : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            
            builder.RegisterType<MyDeviceInfo>().As<IDeviceInfo>().SingleInstance();
            builder.RegisterType<Configuration>().As<IConfiguration>().SingleInstance();
            builder.RegisterType<TelemetryService>().As<ITelemetryService>().SingleInstance();
            builder.RegisterType<ViewNavigationService>().As<INavigationService>().SingleInstance();
        }
    }
}
