using System;
using System.IO;

namespace Web.Endpoints
{
	// TODO - local requests only
	public class ImageEndpoint
	{
		public ImageModel Get(ImageLinkModel model)
		{
			return new ImageModel(File.ReadAllBytes(@"C:\Users\Administrator\Desktop\Servers.png"), "image/png");
		}
	}

	public class ImageLinkModel
	{
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