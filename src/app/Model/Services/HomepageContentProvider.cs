using System;
using System.Linq;
using Raven.Client;

namespace Model.Services
{
	public class HomepageContentProvider : IHomepageContentProvider
	{
		private readonly IDocumentSession session;
		protected HomepageContentProvider Provider;

		public HomepageContentProvider(IDocumentSession session)
		{
			if (session == null) throw new ArgumentNullException("session");

			this.session = session;
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
			return session.Query<HomepageContent>().SingleOrDefault() ?? CreateHomepageContent();
		}

		private HomepageContent CreateHomepageContent()
		{
			var homepageContent = new HomepageContent("Welcome to NTCoding");
			session.Store(homepageContent);
			session.SaveChanges();
			homepageContent = session.Query<HomepageContent>().Where(x => x.ID == homepageContent.ID).Single();
			
			return homepageContent;
		}
	}
}