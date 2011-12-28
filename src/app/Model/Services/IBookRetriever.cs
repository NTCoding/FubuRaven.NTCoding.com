using System.Collections.Generic;
using System.Linq;

namespace Model.Services
{
	public interface IBookRetriever
	{
		IEnumerable<Book> GetReviewedBooks(string genre = null);
		
		IEnumerable<Book> GetWishlistBooks();
	}
}