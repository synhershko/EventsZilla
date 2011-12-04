using System;
using System.Web.Mvc;
using System.Linq;
using EventsZilla.Models;

namespace EventsZilla.Controllers
{
    public class HomeController : ZillaController
    {
        public ActionResult Index()
        {
        	var content = RavenSession.Load<ContentPage>(ContentPage.IdFromSlug("homepage"));
        	return View("HomePage", content);
        }

		public ActionResult FutureEvents()
		{
			var cachableNow = GetCachableNow(); // if we were using DateTimeOffset.Now the query below would have never have cached
			var events = RavenSession.Query<Event>().Where(x => x.StartsAt <= cachableNow).OrderBy(x => x.StartsAt).ToArray();

			ViewBag.Title = "Future events";
			return View("EventsList", events);
		}

		public ActionResult PastEvents()
		{
			var cachableNow = GetCachableNow(); // if we were using DateTimeOffset.Now the query below would have never have cached
			var events = RavenSession.Query<Event>().Where(x => x.EndsAt >= cachableNow).OrderBy(x => x.StartsAt).ToArray();

			ViewBag.Title = "Past events";
			return View("EventsList", events);
		}

		public ActionResult NextEvent()
		{
			var cachableNow = GetCachableNow(); // if we were using DateTimeOffset.Now the query below would have never have cached
			var e = RavenSession.Query<Event>().Where(x => x.StartsAt >= cachableNow).OrderBy(x => x.StartsAt).FirstOrDefault();
			if (e == null)
				return RedirectToAction("Index");

			return RedirectToAction("Index", "Event", new {id = e.Id, slug = e.Slug});
		}

		private static DateTimeOffset GetCachableNow()
		{
			var exactNow = DateTimeOffset.Now;
			return new DateTimeOffset(exactNow.Year, exactNow.Month, exactNow.Day, exactNow.Hour, 0, 0, exactNow.Offset);
		}
    }
}
