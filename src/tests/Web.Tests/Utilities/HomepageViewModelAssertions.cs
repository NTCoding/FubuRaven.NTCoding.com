using System.Collections.Generic;
using System.Linq;
using Model.Services.dtos;
using NUnit.Framework;
using Web.Endpoints.HomepageModels;
using Web.Utilities;

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

		private static bool IsMatch(BlogPostDisplayModel first, BlogPostDTO second)
		{
			return first.Date == second.Date
			       && first.Text_Html == second.Text
			       && first.Title == second.Title;
		}
	}
}