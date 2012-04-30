
using FubuMVC.Core;
using FubuMVC.StructureMap;
using FubuRESTInnovation.Configuration;
using StructureMap;

[assembly: WebActivator.PreApplicationStartMethod(typeof(AppStart), "Start")]
namespace FubuRESTInnovation.Configuration
{
    public class AppStart
    {
        public static void Start()
        {
            var container = new Container();

            BootstrappingExtensions.StructureMap(FubuApplication.For<SevenDizzleRegistry>(), container)
                .Bootstrap();
        }
        
    }
}