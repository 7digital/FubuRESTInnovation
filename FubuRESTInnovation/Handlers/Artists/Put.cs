using System.Net;
using FubuMVC.Core;
using FubuMVC.Core.Http;

namespace FubuRESTInnovation.Handlers.Artists
{
    public class Put
    {
        private readonly IHttpWriter _writer;

        public Put(IHttpWriter writer)
        {
            _writer = writer;
        }

        public ArtistPutResource Invoke(ArtistPutRequest artist)
        {
            ArtistRetriever.Add(new ArtistResource
            {
                Age = artist.Age,
                Name = artist.Name
            });

            _writer.WriteResponseCode(HttpStatusCode.Created, "blah");

            return new ArtistPutResource();
        }
    }

    public class ArtistPutRequest
    {
        [RouteInput]
        public string Name { get; set; }

        public int Age { get; set; }
    }

    public class ArtistPutResource
    {
    }
}