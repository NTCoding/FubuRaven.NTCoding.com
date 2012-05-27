using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Services.dtos;
using Web.Endpoints.Books.ViewModels;
using Web.Utilities;

namespace Web.Endpoints.HomepageModels
{
	public class HomepageViewModel
	{
		public HomepageViewModel(string homepageContent, IEnumerable<BlogPostDTO> recentBlogEntries, IEnumerable<TweetDTO> tweets, IEnumerable<Book> books)
		{
			HomepageContent_Html = homepageContent;
			BlogPosts  = recentBlogEntries.Select(x => new BlogPostDisplayModel(x));
			Tweets     = tweets;
			Books      = books.Select(b => new BookListView(b));
		}

		public String HomepageContent_Html { get; set; }

		public IEnumerable<BlogPostDisplayModel> BlogPosts { get; set; }

		public IEnumerable<TweetDTO> Tweets { get; set; }

		public IEnumerable<BookListView> Books { get; set; }
	}
}