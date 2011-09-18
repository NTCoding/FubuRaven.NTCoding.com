using System;
using NUnit.Framework;
using Web.Tests.Books;

namespace Model.Tests
{
	[TestFixture]
	public class GenreTest
	{
		[Test]
		public void CanCreate()
		{
			new Genre("Cheese");
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void IfEmptyStringSupplied_ShouldThrowException()
		{
			new Genre("");
		}

		[Test]
		public void WhenCreating_ShouldSetName()
		{
			var name = "MyNameIsGenre";
			
			var genre = new Genre(name);

			Assert.AreEqual(name, genre.Name);
		}

		[Test]
		public void ShouldHaveAnID()
		{
			var genre = new Genre("abc");

			var x = genre.Id;
		}
	}
}