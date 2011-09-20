using System.IO;
using System.Web;

namespace Web.Tests.TestDoubles
{
	public class MockHttpPostedFileBase : HttpPostedFileBase
	{
		private readonly byte[] _bytes;

		public MockHttpPostedFileBase(byte[] bytes)
		{
			_bytes = bytes;
		}

		public override Stream InputStream
		{
			get { return new MemoryStream(_bytes); }
		}

		public override int ContentLength
		{
			get { return _bytes.Length; }
		}
	}
}