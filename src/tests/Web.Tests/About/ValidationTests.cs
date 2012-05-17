using System.Collections.Generic;
using System.Linq;
using FubuValidation;
using NUnit.Framework;
using Web.Endpoints.SiteManagement.About;
using Web.Utilities;

namespace Web.Tests.About
{
	[TestFixture]
	public class ValidationTests
	{
		[Test]
		public void No_http_allowed_in_things_I_like_urls()
		{
			var urls = new List<StringWrapper>
			           	{
							new StringWrapper{Text = "http://www.bbc.co.uk"}
			           	};
			var model = new AboutUpdateModel {ThingsILikeUrls = urls};

			var val = Validator.BasicValidator().Validate(model);

			var err = val.MessagesFor<AboutUpdateModel>(m => m.ThingsILikeUrls);

			Assert.That(err.ElementAt(0).GetMessage(), Is.EqualTo("http:// is not a valid prefix for \"Things I Like Urls\""));
		}
	}
}