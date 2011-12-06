using NUnit.Framework;

namespace Model.Tests
{
	[TestFixture]
	public class BookUpdaterTests
	{
		// TODO - should map the properties from dto and update the book - book in session
		[Test]
		public void Update_GivenDtoForBookInSession_ShouldUpdateBookInSession()
		{
			// get a book that exists in session

			// create a dto with different properties using this book's id

			// update the dto

			// assert the results are the same
			Assert.Fail("Get this done");
		}

		// TODo - if the dto has a null image - do not update

		// TODO - if the image is not null - should update it
	}
}