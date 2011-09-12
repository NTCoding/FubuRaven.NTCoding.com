using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Web.Endpoints.SiteManagement;
using Web.Endpoints.SiteManagement.HomepageContentModels;

namespace Web.Tests
{
	[TestFixture]
	public class HomepageContentLinkModelTests
	{
		[Test]
		public void CanCreate()
		{
			new HomepageContentLinkModel();
		}
	}
}
