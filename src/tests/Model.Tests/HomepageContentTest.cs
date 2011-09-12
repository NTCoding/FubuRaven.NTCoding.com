using NUnit.Framework;

namespace Model.Tests
{
	[TestFixture]
	public class HomepageContentTest
	{
		[Test]
		public void CanCreateHomepageContent()
		{
			new HomepageContent("This is the content");
		}

		[Test]
		public void ConstructionShouldSetContent()
		{
			var content = "Content";
			var hpc = new HomepageContent(content);

			Assert.AreEqual(content, hpc.Content);
		}

		[Test]
		public void ShouldInitializeIDTo1()
		{
			var hpc = new HomepageContent("blah");

			Assert.AreEqual("1", hpc.ID);
		}
	}
}