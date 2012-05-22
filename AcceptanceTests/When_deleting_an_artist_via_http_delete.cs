using System.IO;
using System.Net;
using NUnit.Framework;

namespace AcceptanceTess
{
    [TestFixture]
    public class When_deleting_an_artist_via_http_delete
    {
        [Test]
        public void Artist_can_no_longer_be_retrieved_by_get()
        {
            CreateArtistAndVeirfyExists("Baldrick");

            var request = (HttpWebRequest) WebRequest.Create(Requester.BaseUrl + "artists/Baldrick");
            request.Method = "DELETE";
            request.GetResponse();

            Requester.ExamineResponseFor("artists/Baldrick", response =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));   
            });

        }

        // status code is ??

        private void CreateArtistAndVeirfyExists(string name)
        {
            var request = (HttpWebRequest)WebRequest.Create(Requester.BaseUrl + "artists/" + name);
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

           Requester.GetResponse(request);
        }

       
    }
}