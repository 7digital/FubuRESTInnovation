using System;
using System.Net;
using System.Reflection;
using FubuCore.Binding;
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

            return Enum.Parse(enumType, rawValue, true);
        }
    }
}