using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using NUnit.Framework;
using Web.Endpoints;

namespace Web.Tests.Utilities
{
	[TestFixture]
	public class ImageEndpointTests : RavenTestsBase
	{
		[Test]
		public void Get_GivenIdForBook_ShouldReturnBooksImage()
		{
			var endpoint = new ImageEndpoint(Session);
			
			var book = GetBookWithImageFromSession();

			var image = endpoint.Get(new ImageLinkModel {Id = book.Id});

			Assert.AreEqual(book.Image, image.Data);
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
