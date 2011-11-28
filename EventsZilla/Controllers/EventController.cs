using System;
using System.Linq;
using System.Web.Mvc;
using EventsZilla.Models;
using HibernatingRhinos.Loci.Common.Controllers;

namespace EventsZilla.Controllers
{
    public class EventController : RavenController
    {
        public ActionResult Index(int id, string slug)
        {
        	var e = RavenSession.Load<Event>(id);
			if (e == null)
				return HttpNotFound();

			// Make sure the correct slug is being used in the URL
			if (string.IsNullOrEmpty(slug) || !slug.Equals(e.Slug))
				return RedirectToActionPermanent("Index", new {id = id, slug = e.Slug});

            return View(e);
        }

		[HttpPost]
		public ActionResult Register(int id, EventRegistration reg)
		{
			if (!ModelState.IsValid)
				return RedirectToAction("Index", new {id = reg.EventId});

			var e = RavenSession.Load<Event>(reg.EventId);
			if (e == null)
				return HttpNotFound();

			// Check to see if registration window is still open
			var now = DateTimeOffset.Now;
			if (now < e.RegistrationOpens || now > e.RegistrationCloses)
			{
				ViewBag.Message = MvcHtmlString.Create("Registration window has closed");
				return View("Index", e);
			}

			// Verify there are some seats left (WITHOUT waiting for non-stale query results!)
			var eventId = RavenSession.Advanced.GetDocumentId(e);
			var registrantsCount = RavenSession.Query<EventRegistration>().Where(x => x.EventId == eventId).Count();
			if (e.AvailableSeats > 0 && e.AvailableSeats < int.MaxValue && registrantsCount >= e.AvailableSeats)
			{
				ViewBag.Message = MvcHtmlString.Create("No more seats left, sorry!");
				return View("Index", e);
			}

			// All went smooth, save the registration
			RavenSession.Store(reg);

			ViewBag.Message = MvcHtmlString.Create("Registration successful!");
			return View("Index", e);
		}
    }
}
