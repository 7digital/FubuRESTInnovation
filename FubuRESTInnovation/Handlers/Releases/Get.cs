using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core;
using FubuRESTInnovation.Handlers.Artists;

namespace FubuRESTInnovation.Handlers.Releases
{
	public class Get
	{
		public ReleasesViewModel Invoke(ReleasesRequestModel request)
		{
			return new ReleasesViewModel
			       	{
						Releases = ReleaseRetriever.Releases.Where(r => r.Type == request.Type).ToList()
			       	};
		}
	}
	
	public class ReleasesRequestModel
	{
		[QueryString]
		public ReleaseType Type { get; set; }
	}

	public class ReleasesViewModel
	{
		public List<Release> Releases { get; set; }
	}


	public static class ReleaseRetriever
	{
		static ReleaseRetriever()
		{
			Releases = new List<Release>
			           	{
			           		new Release{Id = "111", Type = ReleaseType.Single, ArtistId = "mike"},
			           		new Release{Id = "222", Type = ReleaseType.Single, ArtistId = "mike"},
			           		new Release{Id = "333", Type = ReleaseType.Single, ArtistId = "mike"},
			           		new Release{Id = "444", Type = ReleaseType.Single, ArtistId = "jimmy"},
			           		new Release{Id = "555", Type = ReleaseType.Single, ArtistId = "jimmy"},
			           		new Release{Id = "666", Type = ReleaseType.Single, ArtistId = "jimmy"},
			           		new Release{Id = "Skipadeee", Type = ReleaseType.Album, ArtistId = "Mujanji"},
			           		new Release{Id = "888", Type = ReleaseType.Album, ArtistId = "bob"},
			           		new Release{Id = "999", Type = ReleaseType.Album, ArtistId = "bob"},
			           		new Release{Id = "101", Type = ReleaseType.Album, ArtistId = "bob"},
			           	};
		}

		public static IList<Release> Releases { get; set; }
	}

	public class Release
	{
		public string Id { get; set; }

		public ReleaseType Type { get; set; }

		public string ArtistId { get; set; }
	}

	public enum ReleaseType
	{
		Single,
		Album
	}
}