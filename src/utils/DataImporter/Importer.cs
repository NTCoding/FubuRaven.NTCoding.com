using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;

namespace DataImporter
{
	public class Importer
	{
		private readonly string connectionString;

		public Importer()
		{
			connectionString = ConfigurationManager.AppSettings["connectionString"];
		}

		public IEnumerable<GenreDto> ImportGenres()
		{
			IEnumerable<GenreDto> genres = null;
			Query(c =>
			      	{
						genres = c.Query<GenreDto>("select * from Genres");
			      	});

			return genres;
		}

		public IEnumerable<BookDto> ImportBooks()
		{
			IEnumerable<BookDto> books = null;
			
			Query(c =>
			      	{
			      		books = c.Query<BookDto>("select * from Books");
			      	});

			IEnumerable<AuthorDto> authors = null;
			Query(c =>
			      	{
			      		authors = c.Query<AuthorDto>("select * from Authors A join Authors_Books B on A.ID = B.Authors_ID");
			      	});

			foreach (var bookDto in books)
			{
				bookDto.Authors = authors.Where(a => a.Books_ID == bookDto.Id).ToList().Select(a => a.Name);
			}

			return books;
		}

		private void Query(Action<IDbConnection> action)
		{
			using(var con = new SqlConnection(connectionString))
			{
				con.Open();
				action(con);
			}
		}


		// Import books
			// title
			// id
			// genre id
			// review/description
			// status
			// pull in the authors as a list of string - using a lookup
			// image - copy the images from the site and read them into an array
			// pull in the rating


		// TODO - how am I goin to do the lookup?
			
	}

	public class AuthorDto
	{
		public String Name { get; set; }

		public int Books_ID { get; set; }
	}

	public class BookDto
	{
		public string Description { get; set; }

		public int Genre_Id { get; set; }

		public int Id { get; set; }

		public string ImageUrl { get; set; }

		public int Rating { get; set; }

		public String Status { get; set; }

		public string Title { get; set; }

		public IEnumerable<String> Authors { get; set; }

		public string ImageName
		{
			get
			{
				// get the last index of / in the image url

				var lastInd = ImageUrl.LastIndexOf("/");

				return ImageUrl.Substring(lastInd + 1);
			}
		}
	}

	public class GenreDto
	{
		public string Name { get; set; }

		public int Id { get; set; }
	}
}
