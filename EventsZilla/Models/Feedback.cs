using System;

namespace EventsZilla.Models
{
	public class Feedback
	{
		public string EventId { get; set; }
		public string CommenterName { get; set; }
		public string CommenterEmail { get; set; }
		public string Content { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
	}
}