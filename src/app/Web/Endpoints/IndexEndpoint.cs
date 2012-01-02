using System;
using System.Collections.Generic;
using Model;
using Model.Services;
using Model.Services.dtos;
using Web.Endpoints.HomepageModels;

namespace Web.Endpoints
{
	public class IndexEndpoint
	{
		private readonly IHomepageContentProvider homepageContentProvider;
		private readonly IBlogPostsRetriever blogRetriever;
		private readonly ITweetRetriever tweetRetriever;
		private readonly IBookRetriever bookRetriever;

		public IndexEndpoint(IHomepageContentProvider homepageContentProvider, IBlogPostsRetriever blogRetriever, 
			ITweetRetriever tweetRetriever, IBookRetriever bookRetriever)
		{
			if (homepageContentProvider == null) throw new ArgumentNullException("homepageContentProvider");

			this.homepageContentProvider = homepageContentProvider;
			this.blogRetriever = blogRetriever;
			this.tweetRetriever = tweetRetriever;
			this.bookRetriever = bookRetriever;
		}

		public HomepageViewModel Get(HomepageLinkModel homepageLinkModel)
		{
			var homepageContent = homepageContentProvider.GetHomepageContent();
			var recentBlogEntries = blogRetriever.GetRecentBlogEntries();
			var tweets = tweetRetriever.GetRecentTweets();
			var books = bookRetriever.GetCurrentlyReading();
			
			return new HomepageViewModel(homepageContent, recentBlogEntries, tweets, books);
		}
	}
}