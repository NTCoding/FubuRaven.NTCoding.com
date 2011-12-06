using System;
using System.Linq;
using Raven.Client;

namespace Model.Services
{
	public class HomepageContentProvider : IHomepageContentProvider
	{
		private readonly IDocumentSession _session;
		protected HomepageContentProvider Provider;

		public HomepageContentProvider(IDocumentSession session)
		{
			if (session == null) throw new ArgumentNullException("session");

			_session = session;
		}

		public String GetHomepageContent()
		{
			var homepageContent = GetOrCreateHomepageContentEntity();

			return homepageContent.Content;
		}

		public void SetHomepageContent(string content)
		{
			var homepageContent = GetOrCreateHomepageContentEntity();
			homepageContent.Content = content;
		}

		private HomepageContent GetOrCreateHomepageContentEntity()
		{
			return _session.Query<HomepageContent>().SingleOrDefault() ?? CreateHomepageContent();
		}

		private HomepageContent CreateHomepageContent()
		{
			var homepageContent = new HomepageContent("Welcome to NTCoding");
			_session.Store(homepageContent);
			_session.SaveChanges();
			homepageContent = _session.Query<HomepageContent>().Where(x => x.ID == homepageContent.ID).Single();
			
			return homepageContent;
		}
	}
}