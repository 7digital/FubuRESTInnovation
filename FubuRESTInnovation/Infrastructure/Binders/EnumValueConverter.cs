using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using FubuCore.Binding;
using FubuMVC.Core.Runtime;
using FubuRESTInnovation.Infrastructure.Errors;

namespace FubuRESTInnovation.Infrastructure.Binders
{
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

            try
            {
                return Enum.Parse(enumType, rawValue, true);
            }
            catch (Exception)
            {
                var problems = new List<ConvertProblem>
                {
                    new ConvertProblem { Item = rawValue, Property = context.Property}
                };

                // This is a fubu exception that will build up binding errors in the response 
                // and assign them to the input model (context.Object.GetType())
                throw new BindResultAssertionException(context.Object.GetType(), problems); 
            }
            
        }
    }
}