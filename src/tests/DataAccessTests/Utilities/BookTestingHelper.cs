using System.Linq;
using Model;
using Model.Services.dtos;
using Raven.Client;

namespace DataAccessTests.Utilities
{
	public class BookTestingHelper
	{
		public static Book GetBookFromSessionFor(CreateBookDto dto, IDocumentSession session)
		{
			// TODO - turn this into a RavenDB index
			return session.Query<Book>()
				.Where(b => b.Title == dto.Title)
				.Where(b => b.Genre.Id == dto.Genre)
				.Where(b => b.Review == dto.Description)
				.Where(b => b.Status == dto.Status)
				.Where(b => b.Authors.Any(a => a == dto.Authors.First()))
				.FirstOrDefault();
		}
	}
}