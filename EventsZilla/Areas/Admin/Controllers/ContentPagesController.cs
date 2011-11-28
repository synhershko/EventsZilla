using System.Linq;
using System.Web.Mvc;
using EventsZilla.Core;
using EventsZilla.Models;
using HibernatingRhinos.Loci.Common.Models;
using Raven.Client.Linq;

namespace EventsZilla.Areas.Admin.Controllers
{
    public class ContentPagesController : AdminController
    {
		public ActionResult Index()
		{
			RavenQueryStatistics stats;
			var pages = RavenSession.Query<ContentPage>()
				.Statistics(out stats)
				.Customize(x => x.WaitForNonStaleResultsAsOfLastWrite())
				.ToArray();

			return View("Index", pages);
		}

		[HttpGet]
		public ActionResult Add()
		{
			return View("Edit", new ContentPage { ContentType = DynamicContentType.Markdown });
		}

		[HttpPost]
		public ActionResult Add(ContentPage input)
		{
			if (!ModelState.IsValid)
				return View("Edit", input);

			var pageId = ContentHelpers.FullContentPageId(SlugConverter.TitleToSlug(input.Title));
			var page = RavenSession.Load<ContentPage>(pageId);
			if (page != null)
			{
				ModelState.AddModelError("Id", "Page already exists for the slug you're trying to create it under");
				return View("Edit", input);
			}

			input.Id = pageId;
			RavenSession.Store(input);

			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult Edit(string id)
		{
			var page = RavenSession.Load<ContentPage>(ContentHelpers.FullContentPageId(id));
			if (page == null)
				return HttpNotFound();

			return View("Edit", page);
		}

		[HttpPost]
		public ActionResult Edit(ContentPage input)
		{
			if (!ModelState.IsValid)
				return View("Edit", input);

			var page = string.IsNullOrWhiteSpace(input.Id)
						? null : RavenSession.Load<ContentPage>(ContentHelpers.FullContentPageId(input.Id));
			if (page != null)
			{
				// TODO Use optimistic concurrency?

				page.Content = input.Content;
				page.Title = input.Title;
			}
			else
			{
				// Create new page if none matching found
				input.Id = ContentHelpers.FullContentPageId(SlugConverter.TitleToSlug(input.Title));
				RavenSession.Store(input);
			}

			return RedirectToAction("Index");
		}
    }
}
