using FubuMVC.Core;
using FubuRESTInnovation.Handlers.Releases;

namespace FubuRESTInnovation.Handlers.Artists
{
    public class Get
    {
        public ArtistResource Invoke(ArtistRequest request)
        {
            return null;
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
}