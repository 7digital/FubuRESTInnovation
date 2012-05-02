using System;
using System.Xml.Linq;
using FubuCore.Binding;
using FubuMVC.Core.Http;
using FubuMVC.Core.Runtime;

namespace FubuRESTInnovation.Infrastructure.Binders
{
    public class PutRequestBinder : IModelBinder
    {
        public bool Matches(Type type)
        {
            return type.Name.EndsWith("PutRequest", StringComparison.OrdinalIgnoreCase);
        }

        public object Bind(Type type, IBindingContext context)
        {
            var instance = Activator.CreateInstance(type);

            // let the default binders set all the route values and things
            context.BindProperties(instance);
            
            var data = context.Service<IStreamingData>().InputText();
            
            var x = XDocument.Parse(data);
            
            // TODO - simple implementation. Would be susceptible to overwriting route values from query and url
            foreach (var element in x.Root.Elements())
            {
                SetProperty(type, instance, element, element.Name.ToString());
            }

            return instance;
        }

        private static void SetProperty(Type type, object instance, XElement element, string name)
        {
            var property = type.GetProperty(name);

            // Hacky, just to get something working
            if (property.PropertyType == typeof (int))
            {
                property.SetValue(instance, int.Parse(element.Value), new object[] {});
            }
        }
    }
}