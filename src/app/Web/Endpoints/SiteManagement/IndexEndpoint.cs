namespace Web.Endpoints.SiteManagement
{
	public class IndexEndpoint
	{
		public SiteManagementIndexViewModel Get()
		{
			return new SiteManagementIndexViewModel();
		}
	}

	public class SiteManagementIndexViewModel
	{
	}
}