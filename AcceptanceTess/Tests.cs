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
        [Test]
        public void Homepage_shows_welcome_message()
        {
            var response = WebRequest.Create("http://localhost:59517/").GetResponse();
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                var x = XDocument.Parse(sr.ReadToEnd());
                var message = x.Element("sevendigitalapi").Element("message").Value;

                Assert.That(message, Is.EqualTo("Welcome to the 7digital api"));
            }
        }
    }
}
