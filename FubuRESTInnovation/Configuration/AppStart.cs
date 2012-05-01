
using System;
using System.Reflection;
using FubuCore.Binding;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using FubuRESTInnovation.Configuration;
using FubuRESTInnovation.Handlers.Releases;
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

    public class EnumValueConverter : StatelessConverter
    {
        public override bool Matches(PropertyInfo property)
        {
            var isEnum = property.PropertyType.IsEnum;

            return isEnum;
        }

        public override object Convert(IPropertyContext context)
        {
            var rawValue = context.RawValueFromRequest.RawValue.ToString();

            var enumType = context.Property.PropertyType;

            return Enum.Parse(enumType, rawValue, true);
        }
    }
}