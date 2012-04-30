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
            // when i hit the homepage
            var response = WebRequest.Create("http://localhost/").GetResponse();
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                var x = XDocument.Parse(sr.ReadToEnd());
                var message = x.Element("7digital").Element("message").Value;

                Assert.That(message, Is.EqualTo("Welcome to the 7digital api"));
            }

            // I should get the following xml back

            /*
                 * <7digital>
                 *         <message>
                 *          Welcome to the 7digital api
                 *         </message>
                 * </7digital>
                 * 
                 */

        }
    }
}
