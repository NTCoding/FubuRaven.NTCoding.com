using System;
using System.Linq;
using NUnit.Framework;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;

namespace Web.Tests
{
	[TestFixture]
	public class HomepageContentEndpointTests : RavenTestsBase
	{
		private HomepageContentEndpoint _endpoint;
		private HomepageContentProvider _homepageContentProvider;

		[SetUp]
		public void SetUp()
		{
			_homepageContentProvider = new HomepageContentProvider(_session);
			_endpoint = new HomepageContentEndpoint(_homepageContentProvider);
		}

		[Test]
		public void Post_GivenNewHomepageContent_ShouldSetTheSitesHomepageContent()
		{
			var newContent = "Welcome - Show some love for Fubu and Ravennnnnnn";

			var model = new HomepageContentInputModel {HomepageContent = newContent};

			_endpoint.Post(model);

			Assert.AreEqual(newContent, _homepageContentProvider.GetHomepageContent());
		}

		// TODO - once the content has been set - we need to go back to the home page

		// TODO - add some test for the get scenario - these wouldn't be covered by the specs would they?
	}

	public class HomepageContentEndpoint
	{
		private readonly IHomepageContentProvider _homepageContentProvider;

		public HomepageContentEndpoint(IHomepageContentProvider homepageContentProvider)
		{
			_homepageContentProvider = homepageContentProvider;
		}

		public void Post(HomepageContentInputModel model)
		{
			_homepageContentProvider.SetHomepageContent(model.HomepageContent);
		}
	}

	[TestFixture]
	public class HomepageContentInputModelTests
	{
		[Test]
		public void CanCreate()
		{
			new HomepageContentInputModel();
		}
	}

	public class HomepageContentInputModel
	{
		public String HomepageContent { get; set; }
	}

	[TestFixture]
	public class HomepageContentProviderTests : RavenTestsBase
	{
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SessionCannotBeNull()
		{
			new HomepageContentProvider(null);
		}

		[Test]
		public void GetHomepageContent_ShouldReturnTheCurrentHomepageContent()
		{
			var content = new HomepageContent("Welcome - I am the homepage content");
			
			_session.Store(content);
			_session.SaveChanges();

			Assert.AreEqual(content.Content, _provider.GetHomepageContent());
		}

		[Test]
		public void SetHomepageContent_ShouldSetTheCurrentHomepageContent()
		{
			var content = "this is the new content";

			_provider.SetHomepageContent(content);

			Assert.AreEqual(content, _provider.GetHomepageContent());
		}
	}

	public abstract class RavenTestsBase
	{
		protected HomepageContentProvider _provider;
		protected IDocumentSession _session;
		private EmbeddableDocumentStore _store;

		[SetUp]
		public void SetUp()
		{
			_store = new EmbeddableDocumentStore { DataDirectory = "Data" };
			_store.Initialize();
			_session = _store.OpenSession();

			_provider = new HomepageContentProvider(_session);
		}

		[TearDown]
		public void TearDown()
		{
			_store.DocumentDatabase.TransactionalStorage.Dispose();
		}
	}

	public interface IHomepageContentProvider
	{
		void SetHomepageContent(string homepageContent);
	}

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

	[TestFixture]
	public class HomepageContentTest
	{
		[Test]
		public void CanCreateHomepageContent()
		{
			new HomepageContent("This is the content");
		}

		[Test]
		public void ConstructionShouldSetContent()
		{
			var content = "Content";
			var hpc = new HomepageContent(content);

			Assert.AreEqual(content, hpc.Content);
		}

		[Test]
		public void ShouldInitializeIDTo1()
		{
			var hpc = new HomepageContent("blah");

			Assert.AreEqual("1", hpc.ID);
		}
	}

	public class HomepageContent
	{
		public HomepageContent(string content)
		{
			Content = content;
			ID = "1";
		}

		public String ID { get; set; }

		public String Content { get; protected internal set; }
	}
}
