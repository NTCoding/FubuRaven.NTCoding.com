using System.Linq;
using Raven.Client;

namespace Model.About
{
	public class RavenAboutInfoRetriever : IAboutInfoRetriever
	{
		private readonly IDocumentSession session;

		public RavenAboutInfoRetriever(IDocumentSession session)
		{
			this.session = session;
		}

		public AboutInfo GetAboutInfo()
		{
			return session.Query<AboutInfo>()
			       	.SingleOrDefault() ?? new AboutInfo(string.Empty, Enumerable.Empty<string>());

		}
	}
}