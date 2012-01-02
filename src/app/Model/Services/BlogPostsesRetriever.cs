using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Model.Services.dtos;

namespace Model.Services
{
	public class BlogPostsesRetriever : IBlogPostsRetriever
	{
		private readonly string NTCodingBlogFeedAddress = "http://www.ntcoding.blogspot.com/feeds/posts/default?alt=rss";

		public IEnumerable<BlogPostDTO> GetRecentBlogEntries()
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