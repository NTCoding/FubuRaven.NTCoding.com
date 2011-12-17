using System;

namespace Web.Utilities
{
	public class ImageDisplayModel
	{
		public ImageDisplayModel(String id)
		{
			Id = id;
		}

		public String Id { get; private set; }

		public Int32 Width { get; set; }

		public Int32 Height { get; set; }
	}
}