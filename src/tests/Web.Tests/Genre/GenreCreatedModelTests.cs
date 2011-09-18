using NUnit.Framework;
using Web.Endpoints.SiteManagement.Genre.CreateGenreModels;

[TestFixture]
public class GenreCreatedModelTests
{
	[Test]
	public void CanCreate()
	{
		new GenreCreatedModel("abc");
	}

	[Test]
	public void ShouldConstruct()
	{
		string id = "abc/123";
		var model = new GenreCreatedModel(id);

		Assert.AreEqual(id, model.GenreId);
	}
}