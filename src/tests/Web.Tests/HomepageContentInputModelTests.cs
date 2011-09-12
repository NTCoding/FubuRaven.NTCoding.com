using NUnit.Framework;
using Web.Endpoints.SiteManagement;

namespace Web.Tests
{
	[TestFixture]
	public class HomepageContentInputModelTests
	{
		[Test]
		public void CanCreate()
		{
			new HomepageContentInputModel();
		}
	}
}