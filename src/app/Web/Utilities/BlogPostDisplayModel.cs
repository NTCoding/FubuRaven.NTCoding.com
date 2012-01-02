using System;
using Model.Services.dtos;

namespace Web.Utilities
{
	public class BlogPostDisplayModel
	{
		public BlogPostDisplayModel(BlogPostDTO dto)
		{
			this.Text_Html = dto.Text;
			this.Date = dto.Date;
			this.Title = dto.Title;
		}

		public String Title { get; set; }

		public String Date { get; set; }

		public String Text_Html { get; set; }
	}
}