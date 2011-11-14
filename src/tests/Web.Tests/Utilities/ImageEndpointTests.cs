using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using NUnit.Framework;
using Rhino.Mocks;
using Web.Endpoints;
using Web.Utilities;

namespace Web.Tests.Utilities
{
	[TestFixture]
	public class ImageEndpointTests : RavenTestsBase
	{
		private ImageEndpoint endpoint;
		private ImagePreparer preparer;

		[SetUp]
		public void SetUp()
		{
			preparer = MockRepository.GenerateMock<ImagePreparer>();
			endpoint = new ImageEndpoint(Session, preparer);
		}

		[Test]
		public void Get_GivenIdForBook_ShouldCollaborateWithImagePreparer_ToReturnBooksImage()
		{
			var book = GetBookWithImageFromSession();

			var linkModel = new ImageLinkModel {Id = book.Id};

			endpoint.Get(linkModel);

			preparer.AssertWasCalled(x => x.Prepare(linkModel.Width, linkModel.Height, book.Image, "png"));

			// TODO - verify preparer return data was returned
			//Assert.AreEqual(book.Image, outputModel.Data);
		}

		[Test]
		public void Get_ShouldAlwaysReturnPngs()
		{
			var book = GetBookWithImageFromSession();

			var outputModel = endpoint.Get(new ImageLinkModel {Id = book.Id});

			Assert.AreEqual("image/png", outputModel.ContentType);
		}
		

		private Book GetBookWithImageFromSession()
		{
			var imageData = new byte[] {1, 2, 3, 5, 6, 8, 9};
			var book = BookTestingHelper.GetBook(imageData: imageData);
			Session.Store(book);
			Session.SaveChanges();
			return book;
		}

		// TODO - verify content type is always a png
	}
}
