using FubuMVC.Core;

namespace FubuRESTInnovation.Handlers.Artist
{
    
    public class Get
    {
        public ArtistResource Invoke(ArtistRequest input)
        {
            return new ArtistResource();
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