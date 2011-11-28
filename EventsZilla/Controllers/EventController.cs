using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    }
}
