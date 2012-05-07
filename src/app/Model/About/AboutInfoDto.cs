using System.Collections.Generic;

namespace Model.About
{
	public struct AboutInfoDto
	{
		public string AboutText { get; set; }

		public IEnumerable<string> ThingsILikeUrls { get; set; }
	}
}