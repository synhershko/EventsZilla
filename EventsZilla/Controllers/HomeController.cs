using System;
using System.Web.Mvc;
using System.Linq;
using EventsZilla.Core;
using EventsZilla.Models;
using EventsZilla.ViewModels;

namespace EventsZilla.Controllers
{
    public class HomeController : ZillaController
    {
        public ActionResult Index()
        {
			var cachableNow = GetCachableNow(); // if we were using DateTimeOffset.Now the query below would have never have cached
        	var vm = new HomepageViewModel
        	         	{
							Content = RavenSession.Load<ContentPage>(ContentPage.IdFromSlug("homepage")).Content.CompiledMarkdownContent(),
        	         		PastEvents = RavenSession.Query<Event>().Where(x => x.EndsAt >= cachableNow).OrderBy(x => x.StartsAt).ToList(),
							NextEvent = RavenSession.Query<Event>().Where(x => x.StartsAt >= cachableNow).OrderBy(x => x.StartsAt).FirstOrDefault(),
        	         	};

        	return View("HomePage", vm);
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
