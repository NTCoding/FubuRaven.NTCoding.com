using System.Collections.Generic;
using System.Linq;

namespace Model.Services
{
	// TODO - returning books - not DTO's Baaaaaaaaad!
	public interface IBookRetriever
	{
		IEnumerable<Book> GetReviewedBooks(string genre = null);
		
		IEnumerable<Book> GetWishlistBooks();
		
		Book GetById(string id);
		
		IEnumerable<Book> GetAll();
	}
}