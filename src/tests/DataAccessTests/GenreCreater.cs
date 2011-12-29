using System.Linq;
using DataAccessTests.Utilities;
using Model;
using Model.Services;
using Model.Services.dtos;
using NUnit.Framework;

namespace DataAccessTests
{
	[TestFixture]
	public class GenreCreater : RavenTestsBase
	{
		private RavenDbGenreCreater creater;

		[SetUp]
		public void SetUp()
		{
			creater = new RavenDbGenreCreater(Session);
		}

		[Test]
		public void CanCreate()
		{
			var name = "blah";
			creater.Create(new CreateGenreDto {Name = name});
			Session.SaveChanges();

			var fromSession = Session
				.Query<Genre>()
				.SingleOrDefault(g => g.Name == name);

			Assert.IsNotNull(fromSession);
		}

		[Test]
		public void CannotCreateGenres_WithDuplicateName()
		{
			Assert.Fail();
		}
	}
}