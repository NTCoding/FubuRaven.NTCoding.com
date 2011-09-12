using System;

namespace Model
{
	public class HomepageContent
	{
		public HomepageContent(string content)
		{
			Content = content;
			ID = "1";
		}

		public String ID { get; set; }

		public String Content { get; protected internal set; }
	}
}