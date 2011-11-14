using System;
using System.IO;
using Model;
using Raven.Client;
using Web.Utilities;

namespace Web.Endpoints
{
	// TODO - local requests only
	public class ImageEndpoint
	{
		private readonly IDocumentSession session;

		public ImageEndpoint(IDocumentSession session, ImagePreparer preparer)
		{
			this.session = session;
		}

		public ImageModel Get(ImageLinkModel model)
		{
			var book = session.Load<Book>(model.Id);

			return new ImageModel(book.Image, "image/png");
		}
	}

	public class ImageLinkModel
	{
		public String Id { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }
	}

	public class ImageModel
	{
		public byte[] Data { get; private set; }

		public ImageModel(byte[] data, string contentType)
		{
			Data = data;
			ContentType = contentType;
		}

		public String ContentType { get; private set; }
	}
}