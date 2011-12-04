using System.Linq;
using Raven.Client.Indexes;
using EventsZilla.Models;

namespace EventsZilla.Core
{
	// Index to allow us search for events
	public class Events_BasicIndex : AbstractIndexCreationTask<Event>
	{
		public Events_BasicIndex()
		{
			Map = events => from e in events
			                select new {e.RegistrationOpens, e.RegistrationCloses, e.VenueId, e.StartsAt, e.EndsAt};
		}
	}

	// Index to help us locate registrations by event ID and registrant email
	public class EventRegistrations_BasicIndex : AbstractIndexCreationTask<EventRegistration>
	{
		public EventRegistrations_BasicIndex()
		{
			Map = regs => from r in regs
			              select new {r.EventId, r.RegistrantEmail};
		}
	}
}