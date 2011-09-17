using NUnit.Framework;
using Web.Endpoints.HomepageModels;

namespace Web.Tests.Homepage
{
	[TestFixture]
	public class HomepageViewModelTest
	{
		[Test]
		public void CanCreate()
		{
			new HomepageViewModel();
		}
	}
}