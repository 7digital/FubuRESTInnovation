using System.Net;
using NUnit.Framework;

namespace AcceptanceTess
{
    [TestFixture]
    public class ArtistTests
    {
        [Test]
        public void Responds_with_404_when_artist_does_not_exist()
        {
            Requester.ExamineResponseFor("artist/doesnotexist", 
                r => Assert.That(r.StatusCode, Is.EqualTo(HttpStatusCode.NotFound)));
        }
    }
}