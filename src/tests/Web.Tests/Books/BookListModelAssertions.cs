﻿using System;
using System.Linq;
using Web.Endpoints.SiteManagement.Book.ViewModels;

namespace Web.Tests.Books
{
	public static class BookListModelAssertions
	{
		public static void ShouldContainBookDtoWithId(this BookListModel model, string id)
		{
			if (model.Books.Any(b => b.Id == id)) return;

			throw new Exception("No Dto for book with id: " + id);
		}
	}
}