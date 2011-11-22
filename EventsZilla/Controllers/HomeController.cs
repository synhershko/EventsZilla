using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventsZilla.Models;
using HibernatingRhinos.Loci.Common.Controllers;

namespace EventsZilla.Controllers
{
    public class HomeController : RavenController
    {
        public ActionResult Index()
        {
        	var content = RavenSession.Load<ContentPage>(ContentPage.IdFromSlug("homepage"));
        	return View("ContentPage", content);
        }

    }
}
