using System;
using FubuMVC.Core;

namespace Web.Endpoints.LinkModels
{
	public class ViewBooksLinkModel
	{
		[QueryString]
		public String Genre { get; set; }
	}
}