namespace Web.Endpoints.Authentication
{
	public class LoginModel
	{
		public string Password { get; set; }

		public string User { get; set; }

		public string MagicWord { get; set; }
	}
}