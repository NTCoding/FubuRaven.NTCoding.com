using System;
using NUnit.Framework;

namespace Web.Tests.Books
{
	[TestFixture]
	public class UpdateEndpointTests
	{
		private UpdateEndpoint endpoint;
		// GET 
			// should take a link model with the books id
			
			// should return a book view model with book's details

		[SetUp]
		public void CanCreate()
		{
			endpoint = new UpdateEndpoint();
		}

		[Test]
		public void Get_ShouldTakeALinkModel_WithBooksId()
		{
			endpoint.Get(new UpdateBookLinkModel {Id = "123"});
		}

		// TODO - should blow up if ID Null

		// POST
			// TODO - validation / failure scenario

			// Should create dto and pass to the book updater

			// Should redirect to the view book page
	}

	public class UpdateBookLinkModel
	{
		public String Id { get; set; }
	}

	public class UpdateEndpoint
	{
		public void Get(UpdateBookLinkModel updateBookLinkModel)
		{
		}
	}
}