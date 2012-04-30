using System;
using System.IO;
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
    public class CodecSelector : BasicBehavior
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
            var model = _request.Get<HomeModel>();

            string accept = "";
            _headers.Value<string>("Accept", x => accept = x.Split(',')[0]);

            if (accept == "application/json")
            {
                SetResponseContent("application/json", GetJsonFor(model));
            }
            else
            {
                SetResponseContent("application/xml", GetConventionalXmlFor(model));
            }

            return DoNext.Continue;
        }

        private static string GetJsonFor(HomeModel model)
        {
            return "{\"sevendigitalapi\":" + JsonSerializer.SerializeToString(model) + "}";
        }

        private static string GetConventionalXmlFor(HomeModel model)
        {
            var xml = GetXml(model, model.GetType());

            var conventionalXml = ReplaceModelTypeWithApiRootElement(xml);
            return conventionalXml;
        }

        private void SetResponseContent(string contentType, string content)
        {
            _writer.Write(content);

            _writer.AppendHeader("Content-Type", contentType);
        }

        private static string ReplaceModelTypeWithApiRootElement(string xml)
        {
            var x = XDocument.Parse(xml);
            x.Root.Name = "sevendigitalapi";

            var sww = new StringWriter();
            x.Save(sww);

            var finalXml = sww.ToString();
            return finalXml;
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