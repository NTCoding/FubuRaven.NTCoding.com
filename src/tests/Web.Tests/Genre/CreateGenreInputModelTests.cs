using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Web.Endpoints.SiteManagement.Genre.CreateGenreModels;

namespace Web.Tests.Genre
{
	[TestFixture]
	class CreateGenreInputModelTests
	{
		[Test]
		public void CanCreate()
		{
			new CreateGenreInputModel();
		}
	}
}
