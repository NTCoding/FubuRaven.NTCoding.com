using System;
using System.IO;
using System.Linq;
using Model;
using Raven.Client;
using Web.Utilities;

namespace Web.Endpoints
{
	// TODO - local requests only
	// TODO - maybe cache images?
	public class ImageEndpoint
	{
		private readonly IDocumentSession session;
		private readonly ImagePreparer preparer;

		public ImageEndpoint(IDocumentSession session, ImagePreparer preparer)
		{
			this.session = session;
			this.preparer = preparer;
		}

		public ImageModel Get(ImageLinkModel model)
		{
			var book = session.Load<Book>(model.Id);
			var preparedImage = preparer.Prepare(model.Width, model.Height, book.Image, "png");

			return new ImageModel(preparedImage, "image/png");
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