using System;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Runtime;
using Model;
using Model.Services;
using Web.Endpoints.HomepageModels;
using Web.Endpoints.SiteManagement.HomepageContentModels;

namespace Web.Endpoints.SiteManagement
{
	public class HomepageContentEndpoint
	{
		private readonly IHomepageContentProvider _homepageContentProvider;

		public HomepageContentEndpoint(IHomepageContentProvider homepageContentProvider)
		{
			_homepageContentProvider = homepageContentProvider;
		}

		public FubuContinuation Post(HomepageContentInputModel model)
		{
			_homepageContentProvider.SetHomepageContent(model.HomepageContent);

			return FubuContinuation.RedirectTo<Endpoints.IndexEndpoint>(e => e.Get(new HomepageLinkModel()));
		}

		public HomepageContentViewModel Get(HomepageContentLinkModel model)
		{
			return new HomepageContentViewModel {HomepageContent = _homepageContentProvider.GetHomepageContent()};
		}
	}
}