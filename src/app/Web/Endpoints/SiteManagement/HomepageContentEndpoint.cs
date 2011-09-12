using Model;

namespace Web.Endpoints.SiteManagement
{
	public class HomepageContentEndpoint
	{
		private readonly IHomepageContentProvider _homepageContentProvider;

		public HomepageContentEndpoint(IHomepageContentProvider homepageContentProvider)
		{
			_homepageContentProvider = homepageContentProvider;
		}

		public void Post(HomepageContentInputModel model)
		{
			_homepageContentProvider.SetHomepageContent(model.HomepageContent);
		}
	}
}