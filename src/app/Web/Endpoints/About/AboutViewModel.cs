using System.Collections.Generic;

namespace Web.Endpoints.About
{
	public class AboutViewModel
	{
		public string AboutText { get; set; }

		public IEnumerable<string> ThingsILikeUrls { get; set; }
	}
}