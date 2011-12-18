using System.Collections.Generic;

namespace EventsZilla.Models.SessionArtifacts
{
	public interface ISessionArtifact
	{
	}

	public class ImageGallery : ISessionArtifact
	{
		public List<string> ImageUrls { get; set; }
	}

	public class Link : ISessionArtifact
	{
		public string Title { get; set; }
		public string Url { get; set; }
	}
}