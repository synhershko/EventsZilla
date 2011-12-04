namespace EventsZilla.Models
{
	public class SiteConfig
	{
		public string WebsiteHeader { get; set; }
		public bool RequireRegistration { get; set; }

		public static string ConfigName { get { return "Configs/Site"; } }
	}
}