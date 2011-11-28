using System;
using System.Collections.Generic;
using System.Linq;

namespace EventsZilla.Models
{
	public class Event
	{
		public Event()
		{
			Schedule = new List<ScheduleSlot>();
		}

		public int Id { get; set; }
		public string Title { get; set; }
		public string Slug { get; set; }
		public string Description { get; set; } // markdown content

		public string VenueId { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public DateTimeOffset RegistrationOpens { get; set; }
		public DateTimeOffset RegistrationCloses { get; set; }
		public int AvailableSeats { get; set; }

		public class ScheduleSlot
		{
			public List<string> PresenterIds { get; set; } // list of person IDs
			public string Title { get; set; }
			public string Brief { get; set; } // markdown
			public DateTimeOffset StartingAt { get; set; }
			public DateTimeOffset EndingAt { get; set; }
			
		}
		public List<ScheduleSlot> Schedule { get; set; }

		public DateTimeOffset StartsAt
		{
			get
			{
				var firstSession = Schedule.OrderBy(x => x.StartingAt).FirstOrDefault();
				return firstSession == null ? DateTimeOffset.MinValue : firstSession.StartingAt;
			}
		}

		public DateTimeOffset EndsAt
		{
			get
			{
				var lastSession = Schedule.OrderByDescending(x => x.EndingAt).FirstOrDefault();
				return lastSession == null ? DateTimeOffset.MaxValue : lastSession.EndingAt;
			}
		}
	}
}