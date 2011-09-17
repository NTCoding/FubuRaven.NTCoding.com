using System;
using FubuValidation;

namespace Web.Endpoints.SiteManagement.HomepageContentModels
{
	public class HomepageContentInputModel
	{
		[Required]
		public String HomepageContent { get; set; }
	}
}