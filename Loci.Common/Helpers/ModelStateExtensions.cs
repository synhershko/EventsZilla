// -----------------------------------------------------------------------
//  <copyright file="ModelStateExtensions.cs" company="Hibernating Rhinos LTD">
//      Copyright (c) Hibernating Rhinos LTD. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using System.Web.Mvc;

namespace HibernatingRhinos.Loci.Common.Helpers
{
	public static class ModelStateExtensions
	{
		public static string GetFirstErrorMessage(this ModelStateDictionary modelState)
		{
			var state = modelState.Values
				.Where(v => v.Errors.Count > 0)
				.FirstOrDefault();

			if (state == null) return null;

			var message = state.Errors
				.Where(error => string.IsNullOrEmpty(error.ErrorMessage) == false)
				.Select(error => error.ErrorMessage)
				.FirstOrDefault();
			return message;
		}
	}
}