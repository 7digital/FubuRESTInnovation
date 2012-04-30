using System;
using System.IO;
using System.Net;

static internal class RequestHelper
{
    public static void ExamineResponseFor(string url, Action<string> a, string accept = "")
    {
        var webRequest = (HttpWebRequest)WebRequest.Create(url);

        if (!String.IsNullOrWhiteSpace(accept))
        {
            webRequest.Accept = accept;
        }

        using (var sr = new StreamReader(webRequest.GetResponse().GetResponseStream()))
        {
            a(sr.ReadToEnd());
        }
    }
}