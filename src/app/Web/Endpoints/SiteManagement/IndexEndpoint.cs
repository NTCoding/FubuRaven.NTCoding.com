namespace Web.Endpoints.SiteManagement
{
	public class IndexEndpoint
	{
		public SiteManagementIndexViewModel Get(SiteManagementLinkModel model)
		{
			return new SiteManagementIndexViewModel();
		}
	}

	public class SiteManagementLinkModel
	{
	}

	public class SiteManagementIndexViewModel
	{
	}
}