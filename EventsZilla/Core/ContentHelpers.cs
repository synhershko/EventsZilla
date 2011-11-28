using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HibernatingRhinos.Loci.Common.Models;
using MarkdownDeep;

namespace EventsZilla.Core
{
	public static class ContentHelpers
	{
		public static MvcHtmlString CompiledMarkdownContent(this string str, bool trustContent = false)
		{
			if (str == null) return MvcHtmlString.Empty;

			var md = new Markdown
			         	{
			         		AutoHeadingIDs = true,
			         		ExtraMode = true,
			         		NoFollowLinks = !trustContent,
			         		SafeMode = false,
			         		NewWindowForExternalLinks = true,
			         	};

			var contents = md.Transform(str);
			return MvcHtmlString.Create(contents);
		}
	}
}