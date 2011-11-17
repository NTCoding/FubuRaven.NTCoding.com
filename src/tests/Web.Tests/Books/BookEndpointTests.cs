using NUnit.Framework;

namespace Web.Tests.Books
{
	[TestFixture]
	public class BookEndpointTests
	{
		[Test]
		public void CanCreate()
		{
			var endpoint = new BookEndpoint();
		}

		// Get should show all books in the system
		
	}

	public class BookEndpoint
	{
	}
}