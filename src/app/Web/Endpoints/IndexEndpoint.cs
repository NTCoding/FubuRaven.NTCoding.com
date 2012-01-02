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

		public IndexEndpoint(IHomepageContentProvider homepageContentProvider, IBlogPostsRetriever blogRetriever, ITweetRetriever tweetRetriever)
		{
			if (homepageContentProvider == null) throw new ArgumentNullException("homepageContentProvider");

			this.homepageContentProvider = homepageContentProvider;
			this.blogRetriever = blogRetriever;
			this.tweetRetriever = tweetRetriever;
		}

		public HomepageViewModel Get(HomepageLinkModel homepageLinkModel)
		{
			var homepageContent = homepageContentProvider.GetHomepageContent();
			var recentBlogEntries = blogRetriever.GetRecentBlogEntries();
			var tweets = tweetRetriever.GetRecentTweets();
			
			return new HomepageViewModel(homepageContent, recentBlogEntries, tweets);
		}
	}
}