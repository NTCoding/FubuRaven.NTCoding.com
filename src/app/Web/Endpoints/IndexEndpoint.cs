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
		private readonly IBlogPostRetriever blogRetriever;

		public IndexEndpoint(IHomepageContentProvider homepageContentProvider, IBlogPostRetriever blogRetriever)
		{
			if (homepageContentProvider == null) throw new ArgumentNullException("homepageContentProvider");

			this.homepageContentProvider = homepageContentProvider;
			this.blogRetriever = blogRetriever;
		}

		public HomepageViewModel Get(HomepageLinkModel homepageLinkModel)
		{
			return new HomepageViewModel
			       	{
			       		HomepageContent = homepageContentProvider.GetHomepageContent(),
						BlogPosts = blogRetriever.GetRecentBlogEntries()
			       	};
		}
	}
}