using System;
using System.IO;
using System.Net;

namespace AcceptanceTess
{
    public static class Requester
    {
        private const string BaseUrl = "http://localhost:59517/";

        public static void ExamineResponseBodyFor(string url, Action<string> a, string accept = "")
        {
            using (var sr = new StreamReader(GetRequest(url, accept).GetResponse().GetResponseStream()))
            {
                a(sr.ReadToEnd());
            }
        }

        public static void ExamineResponseFor(string url, Action<HttpWebResponse> a, string accept = "")
        {
            try
            {
                using (var response = (HttpWebResponse)GetRequest(url, accept).GetResponse())
                {
                    a(response);
                }
            }
            catch (WebException ex)
            {
                a((HttpWebResponse) ex.Response);
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