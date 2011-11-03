﻿using System;
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

		[Test]
		public void Construction_GivenABook_ShouldMapGenreName()
		{
			var book = BookTestingHelper.GetBook();

			var model = new ViewBookViewModel(book);

			Assert.AreEqual(book.Genre.Name, model.GenreName);
		}

		[Test]
		public void Construction_GivenABook_ShouldMapDescription()
		{
			var book = BookTestingHelper.GetBook();

			var model = new ViewBookViewModel(book);

			Assert.AreEqual(book.Description, model.Description);
		}

		[Test]
		public void Construction_GivenABook_ShouldMapAuthors()
		{
			var book = BookTestingHelper.GetBook();

			var model = new ViewBookViewModel(book);

			foreach (var author in book.Authors)
			{
				Assert.IsTrue(model.Authors.Any(a => a == author));
			}
		}
	}
}