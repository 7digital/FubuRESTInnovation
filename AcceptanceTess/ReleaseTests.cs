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
        public class When_searching_for_a_release_that_does_exist
        {
            [Test]
            public void Shows_link_to_artist_for_this_release()
            {
                Requester.ExamineResponseBodyFor("/releases/Skipadeee", responseBody =>
                {
                    var x = XDocument.Parse(responseBody);
                    var artistUrl = x.Root.Element("Artist").Element("Url").Value;

                    Assert.That(artistUrl, Is.EqualTo(Requester.BaseUrl + "artists/Mujanji"));
                });
            }
        }

        [TestFixture]
        public class When_searching_for_a_release_that_does_not_exist
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
                Requester.ExamineResponseBodyFor("releases/doesnotexist", responseBody =>
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
                var singles = ReleaseRetriever.Releases.Where(r => r.Type == ReleaseType.Single).ToList();

                Requester.ExamineResponseBodyFor("releases?type=single", responseBody =>
                {
                    var responseReleases = XDocument.Parse(responseBody).Descendants("Release");

                    Assert.That(responseReleases.Count(), Is.EqualTo(singles.Count(t => t.Type == ReleaseType.Single)));

                    foreach (var s in singles)
                    {
                        var match = responseReleases.Single(x => x.Element("Id").Value == s.Id);
                        
                        Assert.That(match.Element("Type").Value, Is.EqualTo(s.Type.ToString()), "No match for: " + s.Id);
                    }
                });
            }
           
            [Test]
            public void Responds_with_400_if_type_value_is_invalid()
            {
                Requester.ExamineResponseFor("releases?type=crap", r => Assert.That(r.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest)));
            }

            [Test]
            public void Shows_parameter_invalid_error_message_if_type_is_invalid()
            {
                Requester.ExamineResponseBodyFor("/releases?type=crap", responseBody =>
                {
                    var x = XDocument.Parse(responseBody);
                    var error = x.Descendants("Error").Single().Value;

                    Assert.That(error, Is.EqualTo("crap is not a valid type"));
                });
            }
            
        }


       
    }
}