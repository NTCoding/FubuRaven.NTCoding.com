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
	public class ImageEndpointTests 
	{
		private ImageEndpoint endpoint;
		private ImagePreparer preparer;

		[SetUp]
		public void SetUp()
		{
			preparer = MockRepository.GenerateMock<ImagePreparer>();
			endpoint = new ImageEndpoint(preparer);
		}

		[Test]
		public void Get_GivenIdForBook_ShouldCallImagePreparerWithBooksDetails()
		{
			var book = GetBookWithImage();

			var linkModel = new ImageLinkModel {Id = book.Id};

			endpoint.Get(linkModel);

			preparer.AssertWasCalled(x => x.Prepare(linkModel.Width, linkModel.Height, book.Image, "png"));
		}

		[Test]
		public void Get_GivenIdForBook_ShouldCollborateWithImagePreparer_ToGetImage()
		{
			var book = GetBookWithImage();
			
			var imageData = new byte[] {1, 2, 3, 4, 5, 6};
			preparer.Stub(x => x.Prepare(1, 1, null, "")).Return(imageData).IgnoreArguments();

			var linkModel = new ImageLinkModel {Id = book.Id};

			var output = endpoint.Get(linkModel);

			Assert.AreEqual(imageData, output.Data);
		}

		[Test]
		public void Get_ShouldAlwaysReturnPngs()
		{
			var book = GetBookWithImage();

			var outputModel = endpoint.Get(new ImageLinkModel {Id = book.Id});

			Assert.AreEqual("image/png", outputModel.ContentType);
		}
		

		private Book GetBookWithImage()
		{
			var imageData = new byte[] {1, 2, 3, 5, 6, 8, 9};
			var book = BookTestingHelper.GetBook(imageData: imageData);

			return book;
		}
	}
}
