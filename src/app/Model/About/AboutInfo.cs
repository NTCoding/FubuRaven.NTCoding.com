using System;
using System.Collections.Generic;

namespace Model.About
{
	public class AboutInfo
	{
		public AboutInfo(string aboutText, IEnumerable<string> thingsILikeUrls)
		{
			AboutText = aboutText;
			ThingsILikeUrls = thingsILikeUrls;
		}

		public string AboutText { get; set; }
		
		public IEnumerable<string> ThingsILikeUrls { get; private set; }

		public string Id { get; set; }
	}
}