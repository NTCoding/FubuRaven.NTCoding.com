using NUnit.Framework;
using Raven.Client;
using Raven.Client.Embedded;

namespace DataAccessTests.Utilities
{
	public abstract class RavenTestsBase
	{
		protected IDocumentSession Session;
		private EmbeddableDocumentStore store;

		[SetUp]
		public void SetUp()
		{
				store = new EmbeddableDocumentStore
				         	{
				         		Configuration =
				         			{
										RunInMemory = true,
				         			}
				         	};

				store.Initialize();

			Session = store.OpenSession();
		}

		[TearDown]
		public void TearDown()
		{
			Session.Dispose();
		}
	}
}