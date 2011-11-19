using NUnit.Framework;

namespace Web.Tests.Books
{
	[TestFixture]
	public class UpdateEndpointTests
	{
		// GET 
			// should take a link model with the books id
			
			// should return a book view model with book's details

		[Test]
		public void CanCreate()
		{
			new UpdateEndpoint();
		}

		// POST
			// TODO - validation / failure scenario

			// Should create dto and pass to the book updater

			// Should redirect to the view book page
	}

	public class UpdateEndpoint
	{
	}
}