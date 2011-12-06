namespace Model.Services
{
	public interface IHomepageContentProvider
	{
		string GetHomepageContent();

		void SetHomepageContent(string homepageContent);
	}
}