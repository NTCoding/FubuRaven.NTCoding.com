using NUnit.Framework;
using Web.Endpoints.SiteManagement.HomepageContentModels;

namespace Web.Tests.Homepage
{
	[TestFixture]
	public class HomepageContentViewModelTests
	{
		[Test]
		public void CanCreate()
		{
			new HomepageContentViewModel();
		}
	}
}
