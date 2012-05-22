using System.IO;
using System.Net;
using System.Xml.Linq;
using NUnit.Framework;

namespace AcceptanceTess
{
    [TestFixture]
    public class When_adding_an_artist_via_http_put
    {
        private const string ARTIST_NAME = "JeremyMiller";
        private HttpWebResponse _response;

        [TestFixtureSetUp]
        public void TFSetup()
        {
            // TODO - once data has been put will stick around for application lifetime - need better test data setup
            var request = (HttpWebRequest)WebRequest.Create(Requester.BaseUrl + "artists/" + ARTIST_NAME);
            request.Method = "PUT";

            using (var wr = new StreamWriter(request.GetRequestStream()))
            {
                var xml = @"
                            <ArtistId>
                                   <Age>20</Age>
                            </ArtistId>
                           ";

                wr.Write(xml);
            }

            _response = Requester.GetResponse(request);
        }

        [Test]
        public void Artists_can_be_requested_via_get_at_url_PUT_to()
        {
            Requester.ExamineResponseBodyFor("artists/" + ARTIST_NAME , responseBody =>
            {
                var x = XDocument.Parse(responseBody);
                var name = x.Root.Element("Name").Value;
                var age = x.Root.Element("Age").Value;

                Assert.That(name, Is.EqualTo(ARTIST_NAME));
                Assert.That(age, Is.EqualTo("20"));
            });
        }

        [Test]
        public void Response_code_is_201_when_artist_succesfully_created()
        {
            Assert.That(_response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        // handle when artist already exists

        // handle an empty put requests that shows template for uploading an artist
    }
}