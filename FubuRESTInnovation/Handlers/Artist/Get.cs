using System.Net;
using System.Web;
using FubuMVC.Core;
using FubuRESTInnovation.Infrastructure.Errors;

namespace FubuRESTInnovation.Handlers.Artist
{
    
    public class Get
    {
        public ArtistResource Invoke(ArtistRequest input)
        {
            var artist = ArtistRetriever.Get(input.Key);

            if (artist == null) throw new ApiException(HttpStatusCode.NotFound, "blah");

            return artist;
        }
    }

    public class ArtistRequest
    {
        [RouteInput]
        public string Key { get; set; }
    }

    public class ArtistResource
    {
    }

    public static class ArtistRetriever
    {
        public static ArtistResource Get(string key)
        {
            return null;
        }
    }
}