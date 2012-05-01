using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using FubuRESTInnovation.Handlers.Releases;
using NUnit.Framework;

namespace AcceptanceTess
{
    [TestFixture]
    public class ReleaseTests
    {
        [TestFixture]
        public class When_searching_for_an_artist_that_does_not_exist
        {
            [Test]
            public void Responds_with_404()
            {
                Requester.ExamineResponseFor("releases/doesnotexist", 
                    r => Assert.That(r.StatusCode, Is.EqualTo(HttpStatusCode.NotFound)));
            }

            [Test]
            public void Shows_release_doesnt_exist_error_message()
            {
                Requester.ExamineResponseBodyFor("releases/doesnotexist",
                    responseBody =>
                        {
                            var x = XDocument.Parse(responseBody);
                            var error = x.Element("sevendigitalapi").Element("Error").Value;
                            Assert.That(error, Is.EqualTo("Release not found: doesnotexist"));
                        });
            }
        }

        [TestFixture]
        public class When_searching_for_a_release_by_type
        {
            [Test]
            public void Returns_releases_of_the_queried_type()
            {
                var releases = ReleaseRetriever.Releases;

                Requester.ExamineResponseBodyFor("releases?type=Single", 
                    responseBody =>
                        {
                            var responseReleases = XDocument.Parse(responseBody).Descendants("Release");

                            Assert.That(responseReleases.Count(), Is.EqualTo(releases.Count(t => t.Type == ReleaseType.Single)));

                            foreach (var r in releases)
                            {
                                var match = responseReleases.Single(x => x.Element("Id").Value == r.Id);
                                
                                Assert.That(match.Element("Type").Value, Is.EqualTo(r.Type), "No match for: " + r.Id);
                            }
                        });
            }

            // error when release type is invalid
            
        }
       
    }
}