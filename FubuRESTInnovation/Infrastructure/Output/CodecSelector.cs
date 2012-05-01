using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Http;
using FubuMVC.Core.Runtime;
using FubuRESTInnovation.Handlers.Home;
using ServiceStack.Text;
using XmlSerializer = System.Xml.Serialization.XmlSerializer;

namespace FubuRESTInnovation.Infrastructure.Output
{
    public class CodecSelector<TOutputModel> : BasicBehavior where TOutputModel : class
    {
        private readonly IFubuRequest _request;
        private readonly IHttpWriter _writer;
        private readonly IRequestHeaders _headers;

        public CodecSelector(IFubuRequest request, IHttpWriter writer, IRequestHeaders headers) : base(PartialBehavior.Ignored)
        {
            _request = request;
            _writer = writer;
            _headers = headers;
        }

        protected override DoNext performInvoke()
        {
            var model = _request.Find<TOutputModel>().First();

            SetResponseFor(model, _headers, _writer);

            return DoNext.Continue;
        }

        public static void SetResponseFor<T>(T model, IRequestHeaders headers, IHttpWriter writer)
        {
            string accept = "";
            headers.Value<string>("Accept", x => accept = x.Split(',')[0]);

            if (accept == "application/json")
            {
                SetResponseContent("application/json", GetJsonFor(model), writer);
            }
            else
            {
                SetResponseContent("application/xml", GetConventionalXmlFor(model), writer);
            }
        }


        /*
         * 
         * 
         * Move all of the below into a new class
         * 
         */


        private static string GetJsonFor(object model)
        {
            return "{\"sevendigitalapi\":" + JsonSerializer.SerializeToString(model) + "}";
        }

        public static string GetConventionalXmlFor(object model)
        {
            var xml = GetXml(model, model.GetType());

            var withSevenDigitalRoot = ReplaceModelTypeWithApiRootElement(xml);

            var withResourceRemoved = StripOutResourceFromNames(withSevenDigitalRoot);

            return withResourceRemoved;
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

            var finalXml = sww.ToString();
            return finalXml;
        }

        private static void SetResponseContent(string contentType, string content, IHttpWriter writer)
        {
            writer.Write(content);

            writer.AppendHeader("Content-Type", contentType);
        }

        private static string GetXml(object model, Type type)
        {
            var sww = new StringWriter();
            var wr = XmlWriter.Create(sww);
            var xs = new XmlSerializer(type);
            xs.Serialize(wr, model);
            
            return sww.ToString();
        }
    }
}