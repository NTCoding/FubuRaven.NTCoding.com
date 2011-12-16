using System;
using FubuMVC.Core;

namespace Web.Endpoints.Books.LinkModels
{
	public class ViewBookLinkModel
	{
		[QueryString]
		public String Id { get; set; }
	}
}