using NUnit.Framework;
using Web.Endpoints.HomepageModels;

namespace Web.Tests.Homepage
{
	[TestFixture]
	public class HomepageLinkModelTests
	{
		[Test]
		public void CanCreate()
		{
			new HomepageLinkModel();
		}
	}
}