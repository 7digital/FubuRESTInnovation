using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using NUnit.Framework;

namespace AcceptanceTess
{
    [TestFixture]
    public class Tests
    {
        private const string BaseUrl = "http://localhost:59517/";
      
        [Test]
        public void Homepage_shows_welcome_message()
        {
            ExamineResponseFor(BaseUrl, (responseBody) =>
            {
                var x = XDocument.Parse(responseBody);
                var message1 = x.Element("sevendigitalapi").Element("Message").Value;

                Assert.That(message1, Is.EqualTo("Welcome to the 7digital api"));
            });
        }

        [Test]
        public void Content_is_json_when_requested_by_header()
        {
            var json = "{\"sevendigitalap\":{\"Message\":\"Welcome to the 7digital api\"}}";

            ExamineResponseFor(BaseUrl, responseBody => Assert.That(json, Is.EqualTo(responseBody)), "application/json");
        }

        private static void ExamineResponseFor(string url, Action<string> a, string accept = "")
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(url);

            if (!string.IsNullOrWhiteSpace(accept))
            {
                webRequest.Accept = accept;
            }

            using (var sr = new StreamReader(webRequest.GetResponse().GetResponseStream()))
            {
                a(sr.ReadToEnd());
            }
        }
    }
}
