using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Services;
using Model.Services.dtos;
using NUnit.Framework;
using Rhino.Mocks;
using Web.Endpoints;
using Web.Endpoints.HomepageModels;

namespace Web.Tests.Homepage
{
	[TestFixture]
	public class IndexEndpointTests
	{
		private IndexEndpoint endpoint;
		private IHomepageContentProvider provider;
		private IBlogPostRetriever blogRetriever;

		[SetUp]
		public void SetUp()
		{
			provider = MockRepository.GenerateStub<IHomepageContentProvider>();
			blogRetriever = MockRepository.GenerateStub<IBlogPostRetriever>();
			endpoint = new IndexEndpoint(provider, blogRetriever);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldThrowExceptionIfNoHomepageContentProviderSupplied()
		{
			new IndexEndpoint(null, blogRetriever);
		}

		[Test]
		public void Get_ShouldBeLinkedToByHomepageLinkModel()
		{
			endpoint.Get(new HomepageLinkModel());
		}

		[Test]
		public void Get_ShouldReturnViewModelWithHomepageContentOn()
		{
			string content = "blah";
			
			provider.Stub(x => x.GetHomepageContent()).Return(content);

			var result = endpoint.Get(new HomepageLinkModel());

			Assert.AreEqual(content, result.HomepageContent);
		}

		[Test]
		public void Get_ShouldGrabRecentBlogEntries_AndShowThemOnViewModel()
		{
			var blogPosts = CreateDummyBlogPostDTOs(5);
			
			blogRetriever.Stub(b => b.GetRecentBlogEntries()).Return(blogPosts);

			var viewModel = endpoint.Get(new HomepageLinkModel());

			viewModel.ShouldContain(blogPosts);
		}

		private IEnumerable<BlogPostDTO> CreateDummyBlogPostDTOs(int amount)
		{
			for (int i = 0; i < amount; i++)
			{
				yield return new BlogPostDTO
				             	{
				             		Title = "Post " + i,
				             		Date = DateTime.Now.AddDays(-i).ToShortDateString(),
				             		Text = "blah, mcblah, blah"
				             	};
			}
		}
	}

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