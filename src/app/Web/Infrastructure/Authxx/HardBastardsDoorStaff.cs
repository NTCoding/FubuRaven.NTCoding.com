using System.Web.Security;

namespace Web.Infrastructure.Authxx
{
	public class HardBastardsDoorStaff : IDoorStaff
	{
		public bool HaveAllowedIn(string userName, string password)
		{
			return FormsAuthentication.Authenticate(userName, password);
		}
	}
}