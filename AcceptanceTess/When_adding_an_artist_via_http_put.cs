using System.IO;
using System.Net;
using System.Xml.Linq;
using NUnit.Framework;

namespace AcceptanceTess
{
    [TestFixture]
    public class When_adding_an_artist_via_http_put
    {
        [Test]
        public void Artists_can_be_requested_via_get_at_url_PUT_to()
        {
            // TODO - once data has been put will stick around for application lifetime - need better test data setup
            var request = (HttpWebRequest) WebRequest.Create(Requester.BaseUrl + "artists/Benjamin");
            request.Method = "PUT";

            using (var wr = new StreamWriter(request.GetRequestStream()))
            {
                var xml = @"
                            <Artist>
                                   <Age>20</Age>
                            </Artist>
                           ";

                wr.Write(xml);
            }

            Requester.GetResponse(request);
            
            Requester.ExamineResponseBodyFor("artists/Benjamin", responseBody =>
            {
                var x = XDocument.Parse(responseBody);
                var name = x.Element("Artist").Element("Name").Value;
                var age = x.Element("Artist").Element("Age").Value;

                Assert.That(name, Is.EqualTo("Benjamin"));
                Assert.That(age, Is.EqualTo(20));

            });

        }

        // respose code should be 201 if it is created

        // handle when artist already exists

        // handle an empty put requests that shows template for uploading an artist

    }
}