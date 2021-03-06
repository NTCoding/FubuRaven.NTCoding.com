using System;
using NUnit.Framework;
using Raven.Client;

namespace DataImporter
{
	[TestFixture]
	public class TestConsole
	{
		[Test][Ignore("Not actual tests")]
		public void ShouldImportGenres()
		{
			var importer = new SqlImporter();
			var genres = importer.ImportGenres();

			foreach (var genreDto in genres)
			{
				Console.WriteLine("Id: " + genreDto.Id + "   Name: " + genreDto.Name);
			}
		}

		[Test][Ignore("Not actual tests")]
		public void ShouldImportBooks()
		{
			var importer = new SqlImporter();
			var books = importer.ImportBooks();

			foreach (var bookDto in books)
			{
				var descritpion = "";
				if (bookDto.Description != null && bookDto.Description.Length > 10)
				{
					descritpion = bookDto.Description.Substring(0, 10);
				}

				Console.WriteLine("Title: " + bookDto.Title + " GenreId: " + bookDto.Genre_Id +
					" Id: " + bookDto.Id + " Rating: " + bookDto.Rating + " Status: " + bookDto.Status
					+ " description: " + descritpion + " Authors: " + string.Join(",", bookDto.Authors)
					+ " Image: " + bookDto.ImageName
					);
			}
		}
	}
}