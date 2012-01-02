using System.Collections.Generic;
using System.Linq;
using Model.Services.dtos;
using NUnit.Framework;
using Web.Endpoints.HomepageModels;

namespace Web.Tests.Utilities
{
	public static class HomepageViewModelAssertions
	{
		public static void ShouldContain(this HomepageViewModel model, IEnumerable<BlogPostDTO> posts)
		{
			foreach (var blogPostDto in posts)
			{
				Assert.That(model.BlogPosts.Any(p =>  IsMatch(p, blogPostDto)));
			}
		}

		private static bool IsMatch(BlogPostDTO first, BlogPostDTO second)
		{
			return first.Date == second.Date
			       && first.Text == second.Text
			       && first.Title == second.Title;
		}
	}
}