using System;
using System.Collections.Generic;
using System.Linq;
using Model;

namespace Web.Endpoints.Books.ViewModels
{
	public class ViewBookViewModel : Web.Endpoints.SiteManagement.Book.ViewModels.ViewBookViewModel
	{
		// TODO - Am I happy with view model inheritance?
		public ViewBookViewModel(Book book, IEnumerable<Book> allBooksForSameGenre) : base(book)
		{
			this.Review_Html = book.Review;

			// TODO - Should this mapping occur inside the view model (taking the dependency) or inside the controller?
			this.RelatedBooks = allBooksForSameGenre
				.Where(b => b.Id != book.Id)
				.Select(r => new BookListView(r));
		}

		public String Review_Html { get; set; }

		public IEnumerable<BookListView> RelatedBooks { get; set; }
	}
}