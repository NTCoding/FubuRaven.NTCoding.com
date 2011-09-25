using System;
using FubuMVC.Core;

namespace Web.Endpoints.SiteManagement.Book.CreateModels
{
	public class ViewBookLinkModel
	{
		[QueryString]
		public String Id { get; set; }
	}
}