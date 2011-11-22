using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using HibernatingRhinos.Loci.Common.Models;
using Newtonsoft.Json;

namespace EventsZilla.Models
{
	/// <summary>
	/// General purpose content page to be used throughout the site
	/// </summary>
    public class ContentPage : IDynamicContent
    {
		public ContentPage()
		{
			LastChanged = DateTimeOffset.Now;
		}

		/// <summary>
		/// Content page Id, essentially a slug
		/// </summary>
		public string Id { get; set; }

		[Required]
		public string Title { get; set; }

		public DateTimeOffset LastChanged { get; set; }

		[Required]
		[AllowHtml]
		[DataType(DataType.MultilineText)]
		public string Content { get; set; }

		[JsonIgnore]
		public string Slug
		{
			get { return Id.Substring(Id.IndexOf('/') + 1); }
			set { }
		}

		public DynamicContentType ContentType { get; set; }

		public static string IdFromSlug(string slug)
		{
			return "contentpages/" + slug;
		}
    }
}
