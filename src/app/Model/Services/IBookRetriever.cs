using System.Collections.Generic;
using System.Linq;

namespace Model.Services
{
	public interface IBookRetriever
	{
		IEnumerable<Book> GetReviewedBooksOrderedByRating(string genre = null);
		
		IEnumerable<Book> GetWishlistBooks();
	}
}