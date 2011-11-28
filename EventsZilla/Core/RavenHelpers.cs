using EventsZilla.Models;

namespace EventsZilla.Core
{
	public static class RavenHelpers
	{
		public static int EventIntId(this EventRegistration e)
		{
			return int.Parse(e.EventId.Substring(e.EventId.LastIndexOf('/') + 1));
		}
	}
}