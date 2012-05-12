using System.Net;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Http;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Security;

namespace Web.Infrastructure.Authxx
{
	public class NTCodingAuthenticationBehaviour : BasicBehavior
	{
		private readonly ISecurityContext securityContext;
		private readonly IHttpWriter writer;

		public NTCodingAuthenticationBehaviour(ISecurityContext securityContext, IHttpWriter writer)
			: base(PartialBehavior.Ignored)
		{
			this.securityContext = securityContext;
			this.writer = writer;
		}

		protected override DoNext performInvoke()
		{
			if (securityContext.IsAuthenticated()) return DoNext.Continue;

			writer.WriteResponseCode(HttpStatusCode.NotFound);

			return DoNext.Stop;
		}
	}
}