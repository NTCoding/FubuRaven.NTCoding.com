using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Web.Endpoints.SiteManagement.Book.ViewModels;
using Web.Tests.Utilities;

namespace Web.Tests.Books
{
	[TestFixture]
	public class ViewBookViewModelTests : RavenTestsBase
	{
		[Test]
		public void CanCreate()
		{
			new ViewBookViewModel(BookTestingHelper.GetBook());
		}

		// TODO - cannot be given a null book

		[Test]
		public void Construction_GivenABook_ShouldMapTitle()
		{
			var book = BookTestingHelper.GetBook();

			var model = new ViewBookViewModel(book);

			Assert.AreEqual(book.Title, model.Title);
		}
	}
}
