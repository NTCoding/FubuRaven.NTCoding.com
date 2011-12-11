using System;
using FubuMVC.Core;

namespace Web.Endpoints.Books.LinkModels
{
	public class ViewBooksLinkModel
	{
		[QueryString]
		public String Genre { get; set; }
	}
}