using System;
using System.Collections.Generic;
using Model.Services.dtos;

namespace Web.Endpoints.HomepageModels
{
	public class HomepageViewModel
	{
		public String HomepageContent { get; set; }

		public IEnumerable<BlogPostDTO> BlogPosts { get; set; }
	}
}