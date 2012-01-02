using System.Collections.Generic;
using System.Linq;

namespace Model.Services
{
	// TODO - returning books - not DTO's Baaaaaaaaad!
	public interface IBookRetriever
	{
		IEnumerable<Book> GetReviewedBooks(string genre = null);
		
		IEnumerable<Book> GetWishlistBooks();
		
		IEnumerable<Book> GetAll();

		IEnumerable<Book> GetCurrentlyReading();

		Book GetById(string id);
	}
}