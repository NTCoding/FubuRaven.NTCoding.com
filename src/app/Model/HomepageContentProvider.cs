using System;
using System.Linq;
using Raven.Client;

namespace Model
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
			var homepageContent = _session.Query<HomepageContent>().SingleOrDefault();

			if (homepageContent == null)
			{
				homepageContent = new HomepageContent("Welcome to NTCoding");
				_session.Store(homepageContent);
				_session.SaveChanges();
			}

			var homepageContentEntity = _session.Query<HomepageContent>().Where(x => x.ID == homepageContent.ID).Single();
			
			return homepageContentEntity;
		}
	}
}