using System;
using System.Web.Mvc;
using EventsZilla.Models;
using HibernatingRhinos.Loci.Common.Controllers;

namespace EventsZilla.Controllers
{
	public class ZillaController : RavenController
	{
		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);
			
			using (RavenSession.Advanced.DocumentStore.AggressivelyCacheFor(TimeSpan.FromMinutes(30)))
			{
				ViewBag.SiteConfig = RavenSession.Load<SiteConfig>(SiteConfig.ConfigName);
			}
		}
	}
}