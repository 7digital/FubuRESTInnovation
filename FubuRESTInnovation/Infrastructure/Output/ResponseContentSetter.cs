using FubuMVC.Core.Http;
using FubuRESTInnovation.Infrastructure.Behaviours;
using ServiceStack.Text;

namespace FubuRESTInnovation.Infrastructure.Output
{
    public static class ResponseContentSetter
    {
        public static void SetResponseContentFor<T>(T model, IRequestHeaders headers, IHttpWriter writer)
        {
            string accept = "";
            headers.Value<string>("Accept", x => accept = x.Split(',')[0]);

            if (accept == "application/json")
            {
                SetResponseContent("application/json", GetJsonFor(model), writer);
            }
            else
            {
                SetResponseContent("application/xml", XmlOutputBuilder.GetConventionalXmlFor(model), writer);
            }
        }

        private static void SetResponseContent(string contentType, string content, IHttpWriter writer)
        {
            writer.Write(content);

            writer.AppendHeader("Content-Type", contentType);
        }

        private static string GetJsonFor(object model)
        {
            return "{\"sevendigitalapi\":" + JsonSerializer.SerializeToString(model) + "}";
        }

        public static void SetErrorResponse(string message, IHttpWriter httpWriter, IRequestHeaders requestHeaders)
        {
            var model = new ErrorResponse {Error = message};
            SetResponseContentFor(model, requestHeaders, httpWriter);
        }
    }
}