using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Web.Utilities
{
	public interface ImagePreparer
	{
		byte[] Prepare(int width, int height, byte[] image, string format);
	}

	public class SimpleImagePreparer : ImagePreparer
	{
		public byte[] Prepare(int width, int height, byte[] image, string format)
		{
			var i = Image.FromStream(new MemoryStream(image));

            // Prevent using images internal thumbnail
           i.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
           i.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

            int newHeight = i.Height * width / i.Width;
            if (newHeight > height)
            {
                // Resize with height instead
                width = i.Width * height / i.Height;
                newHeight = height;
            }

            var newImage = i.GetThumbnailImage(width, newHeight, null, IntPtr.Zero);

            // Clear handle to original file so that we can overwrite it if necessary
            i.Dispose();

            using (var memStream = new MemoryStream())
            {
				if (format != "png") throw new InvalidOperationException("Need to allow other image types");

                newImage.Save(memStream, ImageFormat.Png);
                
				return memStream.ToArray();
            }
		}
	}
}