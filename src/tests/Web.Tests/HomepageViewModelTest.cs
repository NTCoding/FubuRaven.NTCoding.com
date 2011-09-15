using NUnit.Framework;
using Web.Endpoints.HomepageModels;

namespace Web.Tests
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