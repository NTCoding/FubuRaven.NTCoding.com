using System.Net;
using FubuMVC.Core.Http;
using FubuMVC.Core.Security;

namespace Web.Infrastructure.Authxx
{
	public class NTCodingAuthorizationFailureHandler : IAuthorizationFailureHandler
	{
		private readonly IHttpWriter _writer;

		public NTCodingAuthorizationFailureHandler(IHttpWriter writer)
		{
			_writer = writer;
		}

		public void Handle()
		{
			_writer.WriteResponseCode(HttpStatusCode.NotFound);
		}
	}
}