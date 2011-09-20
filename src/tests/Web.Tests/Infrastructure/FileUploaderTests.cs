using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Web.Infrastructure.Services;
using Web.Tests.TestDoubles;

namespace Web.Tests.Infrastructure
{
	[TestFixture]
	public class FileUploaderTests
	{
		[Test]
		public void GetBytes_GivenAHttpPostedFileBaseWithBytes_ShouldReturnThoseBytes()
		{
			var bytes = new List<Byte>();
			for (int i = 0; i < 500; i++)
			{
				bytes.Add((byte)i);
			}

			var file = new MockHttpPostedFileBase(bytes.ToArray());

			var result = FileUploader.GetBytes(file);

			for (int i = 0; i < bytes.Count; i++)
			{
				Assert.AreEqual(bytes[i], result[i]);
			}
		}
	}
}
