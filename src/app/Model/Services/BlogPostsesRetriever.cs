using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Model.Services.dtos;

namespace Model.Services
{
	// TODO - in DDD this implemenatation would probably live outside the domain model?
	public class BlogPostsesRetriever : IBlogPostsRetriever
	{
		private readonly string NTCodingBlogFeedAddress = "http://www.ntcoding.blogspot.com/feeds/posts/default?alt=rss";

		public IEnumerable<BlogPostDTO> GetRecentBlogEntries()
		{
			var t = new Task<IEnumerable<BlogPostDTO>>(GetEntries);
			t.Start();

			var st = new Stopwatch();
			st.Start();
			
			var tenSeconds = 1000 * 10;
			while (st.ElapsedMilliseconds < tenSeconds)
			{
				if (t.IsCompleted) return t.Result;
			}

			return Enumerable.Empty<BlogPostDTO>();
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