namespace Web.Utilities
{
	public interface ImagePreparer
	{
		object Prepare(int width, int height, byte[] image, string format);
	}
}