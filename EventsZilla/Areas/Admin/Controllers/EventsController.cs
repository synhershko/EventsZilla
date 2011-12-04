using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EventsZilla.Core;
using EventsZilla.Models;

namespace EventsZilla.Areas.Admin.Controllers
{
    public class EventsController : AdminController
    {
        public ActionResult Index()
        {
        	var events = RavenSession.Query<Event>().ToArray();
            return View(events);
        }

		[HttpGet]
		public ActionResult Create()
		{
			return View("Edit", new Event {AvailableSeats = 0, RegistrationCloses = DateTimeOffset.MaxValue});
		}

		[HttpPost]
		public ActionResult Create(Event e)
		{
			if (!ModelState.IsValid)
				return View("Edit", e);

			e.Slug = SlugConverter.TitleToSlug(e.Title);
			e.CreatedAt = DateTimeOffset.Now;

			e.Schedule = new List<Event.ScheduleSlot>
			             	{
			             		new Event.ScheduleSlot
			             			{
			             				Brief = "",
										StartingAt = DateTimeOffset.MinValue,
										EndingAt = DateTimeOffset.MaxValue,
										Title = "",
			             			}
			             	};

			RavenSession.Store(e);

			return RedirectToAction("Index", "Event", new {e.Id, e.Slug, @area = ""});
		}
    }
}
