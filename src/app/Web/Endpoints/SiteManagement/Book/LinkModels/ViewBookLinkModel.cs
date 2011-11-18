using System;
using FubuMVC.Core;

namespace Web.Endpoints.SiteManagement.Book.LinkModels
{
	public class ViewBookLinkModel
	{
		[QueryString]
		public String Id { get; set; }
	}
}