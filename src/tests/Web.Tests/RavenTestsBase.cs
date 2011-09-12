using Model;
using NUnit.Framework;
using Raven.Client;
using Raven.Client.Embedded;

namespace Web.Tests
{
	public abstract class RavenTestsBase
	{
		protected HomepageContentProvider Provider;
		protected IDocumentSession Session;
		private EmbeddableDocumentStore _store;

		[SetUp]
		public void SetUp()
		{
			_store = new EmbeddableDocumentStore { DataDirectory = "Data" };
			_store.Initialize();
			Session = _store.OpenSession();

			Provider = new HomepageContentProvider(Session);
		}

		[TearDown]
		public void TearDown()
		{
			_store.DocumentDatabase.TransactionalStorage.Dispose();
		}
	}
}