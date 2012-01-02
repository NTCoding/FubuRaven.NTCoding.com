using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Model.Services;
using Model.Services.dtos;

namespace Web.Utilities
{
	public class BlogPostsesRetriever : IBlogPostsRetriever
	{
		private readonly string NTCodingBlogFeedAddress = "http://www.ntcoding.blogspot.com/feeds/posts/default?alt=rss";

		public IEnumerable<BlogPostDTO> GetRecentBlogEntries()
		{
			// TODO - will we be using the HTTP cache here?
			return AsyncHelper.DoTimedOperation(GetEntries, 10, Enumerable.Empty<BlogPostDTO>());
		}

		private IEnumerable<BlogPostDTO> GetEntries()
		{
			var blog = XDocument.Load(NTCodingBlogFeedAddress);
			var posts = blog.Descendants("item");
			var mostRecentPosts = posts.Take(3);

			return mostRecentPosts.Select(p => new BlogPostDTO
			                                   	{
			                                   		Title = p.Element("title").Value,
			                                   		Date = Convert.ToDateTime(p.Element("pubDate").Value).ToShortDateString(),
			                                   		Text = p.Element("description").Value.Replace("<style>", "")
			                                   	});
		}
	}
}