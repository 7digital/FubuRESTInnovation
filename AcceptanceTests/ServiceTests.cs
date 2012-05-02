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
    public class ServiceTests
    {
        [Test]
        public void Homepage_shows_welcome_message()
        {
            Requester.ExamineResponseBodyFor("", (responseBody) =>
            {
                var x = XDocument.Parse(responseBody);
                var message1 = x.Element("sevendigitalapi").Element("Message").Value;

                Assert.That(message1, Is.EqualTo("Welcome to the 7digital api"));
            });
        }

        [Test]
        public void Content_is_json_when_requested_by_header()
        {
            var json = "{\"sevendigitalapi\":{\"Message\":\"Welcome to the 7digital api\"}}";

            Requester.ExamineResponseBodyFor("", responseBody => Assert.That(responseBody, Is.EqualTo(json)), "application/json");
        }
    }
}
