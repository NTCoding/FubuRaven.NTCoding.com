using System;

namespace Web.Utilities
{
	public class ImageDisplayModel
	{
		public ImageDisplayModel(String id)
		{
			Id = id;

			Width = 300;
			Height = 300;
		}

		public String Id { get; private set; }

		public Int32 Width { get; private set; }

		public Int32 Height { get; private set; }
	}
}