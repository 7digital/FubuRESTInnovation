using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace FubuRESTInnovation.Infrastructure.Output
{
    public static class XmlOutputBuilder
    {
        public static string GetConventionalXmlFor(object model)
        {
            var xml = GetXml(model, model.GetType());

            var withSevenDigitalRoot = ReplaceModelTypeWithApiRootElement(xml);

            var withResourceRemoved = StripOutResourceFromNames(withSevenDigitalRoot);

            return withResourceRemoved;
        }

        private static string GetXml(object model, Type type)
        {
            var sww = new StringWriter();
            var wr = XmlWriter.Create(sww);
            var xs = new XmlSerializer(type);
            xs.Serialize(wr, model);

            return sww.ToString();
        }

        private static string ReplaceModelTypeWithApiRootElement(string xml)
        {
            var x = XDocument.Parse(xml);
            x.Root.Name = "sevendigitalapi";

            return GetStringFrom(x);
        }

      

        private static string StripOutResourceFromNames(string withSevenDigitalRoot)
        {
            var x = XDocument.Parse(withSevenDigitalRoot);

            foreach (var e in x.Descendants())
            {
                e.Name = e.Name.ToString().Replace("Resource", "");
            }

            return GetStringFrom(x);
        }

        private static string GetStringFrom(XDocument x)
        {
            var sww = new StringWriter();
            x.Save(sww);

            return sww.ToString();
        }

    }
}