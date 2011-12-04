using System.Collections.Generic;
using System.Web.Mvc;
using EventsZilla.Models;

namespace EventsZilla.ViewModels
{
	public class HomepageViewModel
	{
		public MvcHtmlString Content { get; set; }
		public Event NextEvent { get; set; }
		public List<Event> PastEvents { get; set; }
	}
}