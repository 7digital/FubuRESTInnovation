using System.Collections.Generic;
using System.Linq;
using System.Net;
using FubuMVC.Core;
using FubuRESTInnovation.Handlers.Releases;
using FubuRESTInnovation.Infrastructure.Errors;

namespace FubuRESTInnovation.Handlers.Artists
{
    public class Get
    {
        public ArtistResource Invoke(ArtistRequest request)
        {
            var artist = ArtistRetriever.Get(request.Key);

            if (artist == null) throw new ApiException(HttpStatusCode.NotFound, "ArtistId " + request.Key + " not found");

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
        public string Name { get; set; }

        public int Age { get; set; }
    }

   

    public static class ArtistRetriever
    {
        private static readonly IList<ArtistResource> _artists  = new List<ArtistResource>();

        public static ArtistResource Get(string name)
        {
            return _artists.SingleOrDefault(a => a.Name == name);
        }

        public static void Add(ArtistResource artistResource)
        {
            _artists.Add(artistResource);
        }


        public static void Delete(string name)
        {
            _artists.Remove(_artists.Single(a => a.Name == name));
        }
    }

   

}