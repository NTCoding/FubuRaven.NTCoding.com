namespace Model.Auth
{
	public interface IDoorStaff
	{
		bool HaveAllowedIn(string userName, string password);
	}
}