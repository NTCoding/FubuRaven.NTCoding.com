using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace DataAccessTests
{
	// TODO - Raven tests base should only exist in this project

	[TestFixture]
	public class BookRetrieverTests
	{
		[Test]
		public void GetReviewedBooks_ShouldOnlyReturnReviewedBooks()
		{
			Assert.Inconclusive();
		}

		[Test]
		public void GivenGenreId_ShouldOnlyGetBooks_ForThatGenre()
		{
			Assert.Inconclusive();
		}

		[Test]
		public void GetWishlistBooks_ShouldOnlyReturnBooks_OnTheWishlist()
		{
			Assert.Inconclusive();
		}

		[Test]
		public void GetById()
		{
			Assert.Fail();
		}
	}
}
