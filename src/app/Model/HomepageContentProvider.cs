using System;
using System.Linq;
using Raven.Client;

namespace Model
{
	public class HomepageContentProvider : IHomepageContentProvider
	{
		private readonly IDocumentSession _session;

		public HomepageContentProvider(IDocumentSession session)
		{
			if (session == null) throw new ArgumentNullException("session");

			_session = session;
		}

		public object GetHomepageContent()
		{
			var homepageContent = GetHomepageContentEntity();

			return homepageContent.Content;
		}

		public void SetHomepageContent(string content)
		{
			var homepageContent = GetHomepageContentEntity();
			homepageContent.Content = content;
		}

		private HomepageContent GetHomepageContentEntity()
		{
			return _session.Query<HomepageContent>().First();
		}
	}
}