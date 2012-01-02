using System;
using System.Collections.Generic;
using System.Linq;
using Model.Services.dtos;
using Web.Utilities;

namespace Web.Endpoints.HomepageModels
{
	public class HomepageViewModel
	{
		public HomepageViewModel(string homepageContent, IEnumerable<BlogPostDTO> recentBlogEntries, IEnumerable<TweetDTO> tweets)
		{
			HomepageContent = homepageContent;
			BlogPosts = recentBlogEntries.Select(x => new BlogPostDisplayModel(x));
			Tweets = tweets;
		}

		public String HomepageContent { get; set; }

		public IEnumerable<BlogPostDisplayModel> BlogPosts { get; set; }

		public IEnumerable<TweetDTO> Tweets { get; set; }
	}
}