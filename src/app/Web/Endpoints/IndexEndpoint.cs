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

		public IndexEndpoint(IHomepageContentProvider homepageContentProvider, IBlogPostsRetriever blogRetriever)
		{
			if (homepageContentProvider == null) throw new ArgumentNullException("homepageContentProvider");

			this.homepageContentProvider = homepageContentProvider;
			this.blogRetriever = blogRetriever;
		}

		public HomepageViewModel Get(HomepageLinkModel homepageLinkModel)
		{
			var homepageContent = homepageContentProvider.GetHomepageContent();
			var recentBlogEntries = blogRetriever.GetRecentBlogEntries();
			
			return new HomepageViewModel(homepageContent, recentBlogEntries);
		}
	}
}