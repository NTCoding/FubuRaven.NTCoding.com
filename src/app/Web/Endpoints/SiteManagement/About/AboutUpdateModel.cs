using System.Collections.Generic;

namespace Web.Endpoints.SiteManagement.About
{
	public class AboutUpdateModel
	{
		public string AboutText { get; set; }

		public List<string> ThingsILikeUrls { get; set; }
	}
}