using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Web.Endpoints.SiteManagement.Book.ViewModels;

namespace Web.Tests.Books
{
	[TestFixture]
	public class ViewBookViewModelTests
	{
		[Test]
		public void CanCreate()
		{
			new ViewBookViewModel();
		}

		// TODO - cannot be given a null book

		[Test][Ignore]
		public void Construction_GivenABook_ShouldMapTitle()
		{
			// create a book
		}
	}
}
