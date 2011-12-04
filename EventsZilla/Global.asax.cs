using System;
using System.Net.Sockets;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using EventsZilla.Models;
using HibernatingRhinos.Loci.Common.Controllers;
using HibernatingRhinos.Loci.Common.Routing;
using HibernatingRhinos.Loci.Common.Tasks;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace EventsZilla
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		private const string MatchPositiveInteger = @"\d{1,10}";

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"Register",
				"register/{id}",
				new { controller = "Event", action = "Register" },
				new { id = MatchPositiveInteger }
			);

			routes.MapRoute(
				"EventsDisplay",
				"event/{id}/{*slug}",
				new { controller = "Event", action = "Index" },
				new { id = MatchPositiveInteger }
			);

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);

		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);

			InitializeRaven();

			RavenController.DocumentStore = _store;
			TaskExecutor.DocumentStore = _store;

			// Work around nasty .NET framework bug
			try
			{
				new Uri("http://fail/first/time?only=%2bplus");
			}
			catch (Exception)
			{
			}
		}

		protected void Application_BeginRequest(Object sender, EventArgs e)
		{
			OrganicSeoHelpers.FixUrlOnBeginRequest(this);
		}

		private static DocumentStore _store;

		private static void InitializeRaven()
		{
			try
			{
				_store = new DocumentStore { ConnectionStringName = "RavenDB" };
				_store.Initialize();

				var generator = new MultiTypeHiLoKeyGenerator(Store, 5);
				Store.Conventions.DocumentKeyGenerator = entity => generator.GenerateDocumentKey(Store.Conventions, entity);

				IndexCreation.CreateIndexes(Assembly.GetCallingAssembly(), _store);

				using (var session = _store.OpenSession())
				{
					if (session.Load<SiteConfig>(SiteConfig.ConfigName) == null)
					{
						session.Store(new SiteConfig { RequireRegistration = true }, SiteConfig.ConfigName);
						session.SaveChanges();
					}
				}

				ConfigureVersioning();
			}
			catch (Exception e)
			{
				if (RedirectToErrorPageIfRavenDbIsDown(e) == false)
					throw;
			}
		}

		private static void ConfigureVersioning()
		{
			using (var s = _store.OpenSession())
			{
				var item = s.Load<dynamic>("Raven/Versioning/DefaultConfiguration");
				if (item == null)
				{
					s.Store(new
					{
						Exclude = true,
						Id = "Raven/Versioning/DefaultConfiguration",
					});
					s.SaveChanges();
				}
			}
		}

		private static bool RedirectToErrorPageIfRavenDbIsDown(Exception e)
		{
			var socketException = e.InnerException as SocketException;
			if (socketException == null)
				return false;

			switch (socketException.SocketErrorCode)
			{
				case SocketError.AddressNotAvailable:
				case SocketError.NetworkDown:
				case SocketError.NetworkUnreachable:
				case SocketError.ConnectionAborted:
				case SocketError.ConnectionReset:
				case SocketError.TimedOut:
				case SocketError.ConnectionRefused:
				case SocketError.HostDown:
				case SocketError.HostUnreachable:
				case SocketError.HostNotFound:
					return true;
				default:
					return false;
			}
		}

		public static IDocumentStore Store
		{
			get
			{
				if (_store == null)
					HttpContext.Current.Response.Redirect("~/RavenNotReachable.htm");
				return _store;
			}
		}
	}
}