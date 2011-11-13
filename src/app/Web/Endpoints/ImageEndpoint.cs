using System;
using System.IO;
using Model;
using Raven.Client;

namespace Web.Endpoints
{
	// TODO - local requests only
	public class ImageEndpoint
	{
		private readonly IDocumentSession session;

		public ImageEndpoint(IDocumentSession session)
		{
			this.session = session;
		}

		public ImageModel Get(ImageLinkModel model)
		{
			// TODO - do we translate to png here?
			var book = session.Load<Book>(model.Id);
			return new ImageModel(book.Image, "");
		}
	}

	public class ImageLinkModel
	{
		public String Id { get; set; }
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