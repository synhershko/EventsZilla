using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DataAnnotationsExtensions;

namespace EventsZilla.Models
{
	public class Feedback
	{
		public string EventId { get; set; }
		public string CommenterName { get; set; }

		[Email]
		public string CommenterEmail { get; set; }

		[Required]
		[AllowHtml]
		[DataType(DataType.MultilineText)]
		public string Content { get; set; }

		public DateTimeOffset CreatedAt { get; set; }
	}
}