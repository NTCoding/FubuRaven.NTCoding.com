using System;
using Model;
using Model.Services;
using Web.Endpoints.HomepageModels;

namespace Web.Endpoints
{
	public class IndexEndpoint
	{
		private readonly IHomepageContentProvider _homepageContentProvider;

		public IndexEndpoint(IHomepageContentProvider homepageContentProvider)
		{
			if (homepageContentProvider == null) throw new ArgumentNullException("homepageContentProvider");

			_homepageContentProvider = homepageContentProvider;
		}

		public HomepageViewModel Get(HomepageLinkModel homepageLinkModel)
		{
			return new HomepageViewModel {HomepageContent = _homepageContentProvider.GetHomepageContent()};
		}
	}
}