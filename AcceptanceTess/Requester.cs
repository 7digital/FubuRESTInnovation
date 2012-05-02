using System;
using System.IO;
using System.Net;

namespace AcceptanceTess
{
    public static class Requester
    {
        public const string BaseUrl = "http://localhost:59517/";

        public static void ExamineResponseBodyFor(string url, Action<string> a, string accept = "")
        {
            var sr = new StreamReader(GetResponse(url, accept).GetResponseStream());
            var responseBody = sr.ReadToEnd();

            Console.Write(responseBody);

            a(responseBody);
        }

        public static void ExamineResponseFor(string url, Action<HttpWebResponse> a, string accept = "")
        {
            a(GetResponse(url, accept));
        }

        private static HttpWebResponse GetResponse(string url, string accept)
        {
            var request = GetRequest(url, accept);

            return GetResponse(request);
        }

        public static HttpWebResponse GetResponse(HttpWebRequest request)
        {
            try
            {
                return (HttpWebResponse) request.GetResponse();
            }
            catch (WebException ex)
            {
                return (HttpWebResponse) ex.Response;
            }
        }

        private static HttpWebRequest GetRequest(string url, string accept)
        {
            var webRequest = (HttpWebRequest) WebRequest.Create(BaseUrl + url);

            if (!String.IsNullOrWhiteSpace(accept))
            {
                webRequest.Accept = accept;
            }
            return webRequest;
        }
    }
}