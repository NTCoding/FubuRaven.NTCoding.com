using System;
using NUnit.Framework;

namespace DataImporter
{
	[TestFixture]
	public class TestConsole
	{
		[Test]
		public void ShouldImportGenres()
		{
			var importer = new Importer();
			var genres = importer.ImportGenres();

			foreach (var genreDto in genres)
			{
				Console.WriteLine("Id: " + genreDto.Id + "   Name: " + genreDto.Name);
			}
		}

		[Test]
		public void ShouldImportBooks()
		{
			var importer = new Importer();
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