using System.Linq;
using System.Net;
using FubuMVC.Core;
using FubuMVC.Core.Urls;
using FubuRESTInnovation.Handlers.Artists;
using FubuRESTInnovation.Infrastructure.Errors;

namespace FubuRESTInnovation.Handlers.Releases
{
	public class Index
	{
		private IUrlRegistry _urlRegistry;

		public Index(IUrlRegistry urlRegistry)
		{
			_urlRegistry = urlRegistry;
		}

		public ReleaseViewModel Invoke(ReleaseRequest request)
		{
			var release = ReleaseRetriever.Releases.SingleOrDefault(r => r.Id == request.Id);

			if (release == null) throw new ApiException(HttpStatusCode.NotFound, "Release not found: " + request.Id);

			return new ReleaseViewModel
			       	{
						Id = release.Id,
						Type = release.Type,
						Artist = GetArtist(release)
			       	};
		}

		private Artist GetArtist(Release release)
		{
			return new Artist
			             	{
								// TODO - LOL
			             		Url = "http://localhost:59517" + _urlRegistry.UrlFor(new ArtistRequest{Key = release.ArtistId})
			             	};
		}
	}

	public class Artist
	{
		public string Url { get; set; }
	}

	public class ReleaseRequest
	{
		[RouteInput]
		public string Id { get; set; }
	}

	public class ReleaseViewModel
	{
		public string Id { get; set; }

		public ReleaseType Type { get; set; }

		public Artist Artist { get; set; }
	}
}