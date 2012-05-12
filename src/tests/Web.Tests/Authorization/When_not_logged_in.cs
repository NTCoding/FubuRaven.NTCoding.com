using System;
using System.Net;
using NUnit.Framework;

namespace Web.Tests.Authorization
{
	[TestFixture]
	public class When_not_logged_in
	{
		[Test]
		public void When_attempting_to_access_site_management_404s()
		{
			var req = WebRequest.Create("http://localhost:8308/sitemanagement");

			var res = GetResponse(req);

			Assert.That(res.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
		}

		private HttpWebResponse GetResponse(WebRequest req)
		{
			try
			{
				return (HttpWebResponse)req.GetResponse();
			}
			catch (WebException ex)
			{
				return (HttpWebResponse)ex.Response;
			}
		}
	}
}