using System.Collections.Generic;
using System.Web;
using Microsoft.Isam.Esent.Interop;

namespace Web.Infrastructure.Services
{
	public static class FileUploader
	{
		public static byte[] GetBytes(HttpPostedFileBase file)
		{
			var buffer = new byte[file.ContentLength];
			file.InputStream.Read(buffer, 0, buffer.Length);

			return buffer;
		}
	}
}