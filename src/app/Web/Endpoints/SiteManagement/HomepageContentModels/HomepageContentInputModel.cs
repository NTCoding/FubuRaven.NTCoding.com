using System;
using FubuValidation;

namespace Web.Endpoints.SiteManagement.HomepageContentModels
{
	public class HomepageContentInputModel
	{
		[Required]
		public String HomepageContent_BigText { get; set; }
	}
}