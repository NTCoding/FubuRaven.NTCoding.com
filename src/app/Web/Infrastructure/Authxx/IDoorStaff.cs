namespace Web.Infrastructure.Authxx
{
	public interface IDoorStaff
	{
		bool HaveAllowedIn(string userName, string password);
	}
}