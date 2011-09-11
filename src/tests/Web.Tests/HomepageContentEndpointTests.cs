using System;
using System.Linq;
using NUnit.Framework;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;

namespace Web.Tests
{
	[TestFixture]
	public class HomepageContentEndpointTests
	{
		[Test]
		public void CanCreate()
		{
			new HomepageContentEndpoint();
		}

		[Test][Ignore]
		public void Post_GivenNewHomepageContent_ShouldSetTheSitesHomepageContent()
		{
			// create the content
			var newContent = "Welcome - Show some love for Fubu and Ravennnnnnn";

			// stick in a model
			var model = new HomepageContentInputModel();
			
			var endpoint = new HomepageContentEndpoint();

			// invoke the post
			endpoint.Post(model);

			// get the homepage content
			//var homepageContent = new HomepageContentDoobery().GetHomepageContent();

			// assert it matches are custom content
			//Assert.AreEqual(newContent, homepageContent);
		}

		// TODO - once the content has been set - we need to go back to the home page

		// TODO - add some test for the get scenario - these wouldn't be covered by the specs would they?
	}

	public class HomepageContentEndpoint
	{
		public void Post(HomepageContentInputModel model)
		{
			throw new NotImplementedException();
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
	}

	[TestFixture]
	public class HomepageContentDooberyTest
	{
		private HomepageContentDoobery _doobery;
		private IDocumentSession _session;
		private EmbeddableDocumentStore _store;

		[SetUp]
		public void SetUp()
		{
			_store = new EmbeddableDocumentStore { DataDirectory = "Data" };
			_store.Initialize();
			_session = _store.OpenSession();

			_doobery = new HomepageContentDoobery(_session);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SessionCannotBeNull()
		{
			new HomepageContentDoobery(null);
		}

		[TearDown]
		public void TearDown()
		{
			_store.DocumentDatabase.TransactionalStorage.Dispose();
		}

		[Test]
		public void GetHomepageContent_ShouldReturnTheCurrentHomepageContent()
		{
			var content = new HomepageContent("Welcome - I am the homepage content");
			
			_session.Store(content);
			_session.SaveChanges();

			Assert.AreEqual(content.Content, _doobery.GetHomepageContent());
		}
	}

	public class HomepageContentDoobery
	{
		private readonly IDocumentSession _session;

		public HomepageContentDoobery(IDocumentSession session)
		{
			if (session == null) throw new ArgumentNullException("session");

			_session = session;
		}

		public object GetHomepageContent()
		{
			var homepageContent = _session.Query<HomepageContent>().First();

			return homepageContent.Content;
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

		public String Content { get;  private set; }
	}
}
