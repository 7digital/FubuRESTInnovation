using FubuCore.Binding;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using FubuRESTInnovation.Configuration;
using FubuRESTInnovation.Handlers.Releases;
using FubuRESTInnovation.Infrastructure.Binders;
using StructureMap;

[assembly: WebActivator.PreApplicationStartMethod(typeof(AppStart), "Start")]
namespace FubuRESTInnovation.Configuration
{
    public class AppStart
    {
        public static void Start()
        {
            var container = new Container();

            container.Configure(x =>
            {
                x.For<IConverterFamily>().Use<EnumValueConverter>();
            });

            BootstrappingExtensions.StructureMap(FubuApplication.For<SevenDizzleRegistry>(), container)
                .Bootstrap();
        }
        
    }
}