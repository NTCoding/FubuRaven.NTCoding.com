using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using Model.Services;
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
		private IBookRetriever retriever;

		[SetUp]
		public void SetUp()
		{
			preparer = MockRepository.GenerateMock<ImagePreparer>();
			retriever = MockRepository.GenerateMock<IBookRetriever>();
			endpoint = new ImageEndpoint(preparer, retriever);
		}

		[Test]
		public void Get_GivenIdForBook_ShouldCallImagePreparerWithBooksDetails()
		{
			var book = GetBookWithImageSimulatedToExist();

			var linkModel = new ImageLinkModel {Id = book.Id};

			endpoint.Get(linkModel);

			preparer.AssertWasCalled(x => x.Prepare(linkModel.Width, linkModel.Height, book.Image, "png"));
		}

		[Test]
		public void Get_GivenIdForBook_ShouldCollborateWithImagePreparer_ToGetImage()
		{
			var book = GetBookWithImageSimulatedToExist();
			
			var imageData = new byte[] {1, 2, 3, 4, 5, 6};
			preparer.Stub(x => x.Prepare(1, 1, null, "")).Return(imageData).IgnoreArguments();

			var linkModel = new ImageLinkModel {Id = book.Id};

			var output = endpoint.Get(linkModel);

			Assert.AreEqual(imageData, output.Data);
		}

		[Test]
		public void Get_ShouldAlwaysReturnPngs()
		{
			var book = GetBookWithImageSimulatedToExist();

			var outputModel = endpoint.Get(new ImageLinkModel {Id = book.Id});

			Assert.AreEqual("image/png", outputModel.ContentType);
		}
		

		private Book GetBookWithImageSimulatedToExist()
		{
			var imageData = new byte[] {1, 2, 3, 5, 6, 8, 9};
			var book = BookTestingHelper.GetBook(imageData: imageData);
			book.Id = "books/blah";
			retriever.Stub(r => r.GetById(book.Id)).Return(book);

			return book;
		}
	}
}
