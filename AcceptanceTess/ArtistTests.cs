using System.Net;
using System.Xml.Linq;
using NUnit.Framework;

namespace AcceptanceTess
{
    [TestFixture]
    public class ArtistTests
    {
        [TestFixture]
        public class When_searching_for_an_artist_that_does_not_exist
        {
            [Test]
            public void Responds_with_404()
            {
                Requester.ExamineResponseFor("artist/doesnotexist", 
                    r => Assert.That(r.StatusCode, Is.EqualTo(HttpStatusCode.NotFound)));
            }

            [Test]
            public void Shows_artist_doesnt_exist_error_message()
            {
                Requester.ExamineResponseBodyFor("artist/doesnotexist",
                    responseBody =>
                        {
                            var x = XDocument.Parse(responseBody);
                            var error = x.Element("sevendigitalapi").Element("Error").Value;
                            Assert.That(error, Is.EqualTo("Artist not found: doesnotexist"));
                        });
            }
        }
       

        
    }
}