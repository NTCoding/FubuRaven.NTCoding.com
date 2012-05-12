using System.Linq;
using Raven.Client;

namespace Model.About
{
	public class RavenAboutInfoUpdater : IAboutInfoUpdater
	{
		private readonly IDocumentSession session;

		public RavenAboutInfoUpdater(IDocumentSession session)
		{
			this.session = session;
		}

		public void Update(AboutInfoDto info)
		{
			var currentData = session
				.Advanced
				.LuceneQuery<AboutInfo>()
				.WaitForNonStaleResults()
				.FirstOrDefault();

			if (currentData == null)
				session.Store(new AboutInfo(info.AboutText, info.ThingsILikeUrls));
			else
			{
				currentData.AboutText       = info.AboutText;
				currentData.ThingsILikeUrls = info.ThingsILikeUrls;
			}
		}
	}
}