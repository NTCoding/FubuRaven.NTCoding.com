using System;
using System.Collections.Generic;
using System.Linq;
using Model.Services.dtos;
using Web.Utilities;

namespace Web.Endpoints.HomepageModels
{
	public class HomepageViewModel
	{
		public HomepageViewModel(string homepageContent, IEnumerable<BlogPostDTO> recentBlogEntries)
		{
			HomepageContent = homepageContent;
			BlogPosts = recentBlogEntries.Select(x => new BlogPostDisplayModel(x));
		}

		public String HomepageContent { get; set; }

		public IEnumerable<BlogPostDisplayModel> BlogPosts { get; set; }
	}
}