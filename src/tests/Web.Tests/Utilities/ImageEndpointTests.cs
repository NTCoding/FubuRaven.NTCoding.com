using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
			var imageData = new byte[] {1, 2, 3, 5, 6, 8, 9};
			var book = BookTestingHelper.GetBook(imageData: imageData);
			Session.Store(book);
			Session.SaveChanges();

			var endpoint = new ImageEndpoint(Session);

			var image = endpoint.Get(new ImageLinkModel {Id = book.Id});

			Assert.AreEqual(book.Image, image.Data);
		}

		// TODO - verify content type is always a png
	}
}
