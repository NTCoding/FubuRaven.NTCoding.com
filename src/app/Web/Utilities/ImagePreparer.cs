namespace Web.Utilities
{
	public interface ImagePreparer
	{
		byte[] Prepare(int width, int height, byte[] image, string format);
	}
}