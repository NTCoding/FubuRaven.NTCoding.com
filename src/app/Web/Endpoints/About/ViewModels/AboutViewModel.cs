using System.Collections.Generic;

namespace Web.Endpoints.About.ViewModels
{
	public class AboutViewModel
	{
		public string AboutText_Html { get; set; }

		public IEnumerable<string> ThingsILikeUrls { get; set; }
	}
}