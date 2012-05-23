using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using Dapper;

namespace DataImporter
{
	public class SqlImporter
	{
		private readonly string connectionString;

		public SqlImporter()
		{
			connectionString = @"Data Source=VISTA-ULTIMATE\ADVANCEDR2;Initial Catalog=NTCoding;Trusted_Connection=True;";
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
				bookDto.ImageData = GetImageData(bookDto);
			}

			return books;
		}

		private byte[] GetImageData(BookDto bookDto)
		{
			var srcFolder = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent;
			var dataImporterRoot = srcFolder.GetDirectories("utils").Single().GetDirectories("DataImporter").Single();
			var imageDir = new DirectoryInfo(Path.Combine(dataImporterRoot.FullName, "Images"));

			return File.ReadAllBytes(Path.Combine(imageDir.FullName, bookDto.ImageName).Replace(@"Growing Object-Oriented Software, Guided by Tests7b57d8a8-c82e-4cc7-b38c-9f31f47a1f32.jpg", "GOOS.jpg"));

		}

		private void Query(Action<IDbConnection> action)
		{
			using(var con = new SqlConnection(connectionString))
			{
				con.Open();
				action(con);
			}
		}
			
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
				var lastInd = ImageUrl.LastIndexOf("/");

				return ImageUrl.Substring(lastInd + 1);
			}
		}

		public byte[] ImageData { get; set; }
	}

	public class GenreDto
	{
		public string Name { get; set; }

		public int Id { get; set; }
	}
}
