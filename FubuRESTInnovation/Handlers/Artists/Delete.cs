using FubuMVC.Core;

namespace FubuRESTInnovation.Handlers.Artists
{
    public class Delete
    {
        public ArtistDeleteResource Invoke (ArtistDeleteRequest input)
        {
            ArtistRetriever.Delete(input.Name);

            return new ArtistDeleteResource();
        }
    }

    public class ArtistDeleteRequest
    {
        [RouteInput]
        public string Name { get; set; }
    }

    public class ArtistDeleteResource
    {
    }
}