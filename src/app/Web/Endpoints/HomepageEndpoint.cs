using System;
using Model;
using Web.Endpoints.HomepageModels;

namespace Web.Endpoints
{
	public class HomepageEndpoint
	{
		private readonly IHomepageContentProvider _homepageContentProvider;

		public HomepageEndpoint(IHomepageContentProvider homepageContentProvider)
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