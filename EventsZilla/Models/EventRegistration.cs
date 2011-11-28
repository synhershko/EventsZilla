using System;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace EventsZilla.Models
{
	public class EventRegistration
	{
		[Required]
		public string EventId { get; set; }

		[Email]
		[Required]
		[Display(Name = "Your email")]
		public string RegistrantEmail { get; set; }

		[Required]
		[StringLength(50, MinimumLength = 4)]
		[Display(Name = "Your name")]
		public string RegistrantName { get; set; }

		public DateTimeOffset RegisteredAt { get; set; }
	}
}