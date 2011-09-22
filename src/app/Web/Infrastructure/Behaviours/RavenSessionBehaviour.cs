using FubuMVC.Core.Behaviors;
using Microsoft.Practices.ServiceLocation;
using Raven.Client;

namespace Web.Infrastructure.Behaviours
{
	public class RavenSessionBehaviour : BasicBehavior
	{
		private readonly IDocumentSession _session;

		public RavenSessionBehaviour(IDocumentSession session) : base(PartialBehavior.Ignored)
		{
			_session = session;
		}

		protected override void afterInsideBehavior()
		{
			_session.SaveChanges();
			_session.Dispose();
		}
	}
}