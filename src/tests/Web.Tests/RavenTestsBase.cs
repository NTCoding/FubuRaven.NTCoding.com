using Model;
using NUnit.Framework;
using Raven.Client;
using Raven.Client.Embedded;

namespace Web.Tests
{
	public abstract class RavenTestsBase
	{
		protected IDocumentSession Session;
		private EmbeddableDocumentStore _store;

		[SetUp]
		public void SetUp()
		{
				_store = new EmbeddableDocumentStore
				         	{
				         		Configuration =
				         			{
				         				RunInUnreliableYetFastModeThatIsNotSuitableForProduction = true,
										RunInMemory = true,
				         			}
				         	};

				_store.Initialize();

			Session = _store.OpenSession();
		}

		[TearDown]
		public void TearDown()
		{
			_store.DocumentDatabase.TransactionalStorage.Dispose();
		}
	}
}