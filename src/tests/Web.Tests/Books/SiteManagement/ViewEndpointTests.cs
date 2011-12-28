using System.Linq;
using Model;
using Model.Services;
using NUnit.Framework;
using Rhino.Mocks;
using Web.Endpoints.SiteManagement.Book;
using Web.Endpoints.SiteManagement.Book.LinkModels;
using Web.Tests.Utilities;

namespace Web.Tests.Books.SiteManagement
{
	// TODO - Class would not exist using the endpoint testing pattern
	[TestFixture]
	public class ViewEndpointTests 
	{
		private ViewEndpoint _endpoint;
		private IBookRetriever retriever;

		private Book GetBookSimulatedToExist()
		{
			var book = BookTestingHelper.GetBook();
			book.Id = "book/88";
			retriever.Stub(r => r.GetById(book.Id)).Return(book);

			return book;
		}

		[SetUp]
		public void CanCreate()
		{
			retriever = MockRepository.GenerateMock<IBookRetriever>();
			_endpoint = new ViewEndpoint(retriever);
		}

		[Test]
		public void Get_GivenModelWithBooksID_ViewModelShouldContainBooksTitle()
		{
			var book1 = GetBookSimulatedToExist();

			var model = _endpoint.Get(new ViewBookLinkModel { Id = book1.Id });

			Assert.AreEqual(book1.Title, model.Title);
		}

		[Test]
		public void Get_GivenModelWithBooksID_ViewModelSholdContainBooksGenresName()
		{
			var book1 = GetBookSimulatedToExist();

			var model = _endpoint.Get(new ViewBookLinkModel {Id = book1.Id});

			Assert.AreEqual(book1.Genre.Name, model.GenreName);
		}

		[Test]
		public void Get_GivenModelWithBooksID_ViewModelShouldContainBooksDescription()
		{
			var book1 = GetBookSimulatedToExist();

			var model = _endpoint.Get(new ViewBookLinkModel {Id = book1.Id});

			Assert.AreEqual(book1.Review, model.Description_Html);
		}

		[Test]
		public void Get_GivenModelWithBooksID_ViewModelShouldContainBooksStatus()
		{
			var book = GetBookSimulatedToExist();

			var model = _endpoint.Get(new ViewBookLinkModel {Id = book.Id});

			Assert.AreEqual(book.Status.ToString(), model.Status);
		}

		[Test]
		public void Get_ViewModelShouldContainBooksId()
		{
			var book = GetBookSimulatedToExist();

			var model = _endpoint.Get(new ViewBookLinkModel {Id = book.Id});

			Assert.AreEqual(book.Id, model.Id);
		}

		[Test]
		public void Get_GivenModelWithBooksID_ViewModelShouldContainBooksAuthorsNames()
		{
			var book = GetBookSimulatedToExist();

			var model = _endpoint.Get(new ViewBookLinkModel {Id = book.Id});

			foreach (var author in book.Authors)
			{
				Assert.IsTrue(model.Authors.Any(a => a == author));
			}
		}

		// should contain the book's ID - so we can get the image
	}
}