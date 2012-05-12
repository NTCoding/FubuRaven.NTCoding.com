using System.Net;
using NUnit.Framework;

namespace Web.Tests.Authentication
{
	[TestFixture]
	public class When_not_logged_in
	{
		[Test]
		public void Access_to_site_management_404s()
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