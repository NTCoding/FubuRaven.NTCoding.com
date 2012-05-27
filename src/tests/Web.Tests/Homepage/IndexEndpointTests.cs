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
using Web.Tests.Books.Public;
using Web.Tests.Utilities;

namespace Web.Tests.Homepage
{
	[TestFixture]
	public class IndexEndpointTests
	{
		private IndexEndpoint endpoint;
		private IHomepageContentProvider provider;
		private IBlogPostsRetriever blogRetriever;
		private ITweetRetriever tweetRetriever;
		private IBookRetriever bookRetriever;

		[SetUp]
		public void SetUp()
		{
			provider = MockRepository.GenerateStub<IHomepageContentProvider>();
			blogRetriever = MockRepository.GenerateStub<IBlogPostsRetriever>();
			tweetRetriever = MockRepository.GenerateStub<ITweetRetriever>();
			bookRetriever = MockRepository.GenerateStub<IBookRetriever>();
			endpoint = new IndexEndpoint(provider, blogRetriever, tweetRetriever, bookRetriever);
		}

		[Test]
		public void Get_ShouldBeLinkedToByHomepageLinkModel()
		{
			blogRetriever.ReturnEmptyCollectionSoDoesntBreakTest();
			bookRetriever.ReturnEmptyCurrentlyReadingSoDoesntBreakTest();

			endpoint.Get(new HomepageLinkModel());
		}

		[Test]
		public void Get_ShouldReturnViewModelWithHomepageContentOn()
		{
			bookRetriever.ReturnEmptyCurrentlyReadingSoDoesntBreakTest();
			blogRetriever.ReturnEmptyCollectionSoDoesntBreakTest();

			string content = "blah";
			
			provider.Stub(x => x.GetHomepageContent()).Return(content);

			var result = endpoint.Get(new HomepageLinkModel());

			Assert.AreEqual(content, result.HomepageContent_Html);
		}

		[Test]
		public void Get_ShouldGrabRecentBlogEntries_AndShowThemOnViewModel()
		{
			bookRetriever.ReturnEmptyCurrentlyReadingSoDoesntBreakTest();

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

		[Test]
		public void Get_ShouldGrabRecentTweets_AndPutThemOnViewModel()
		{
			blogRetriever.ReturnEmptyCollectionSoDoesntBreakTest();
			bookRetriever.ReturnEmptyCurrentlyReadingSoDoesntBreakTest();

			var tweets = CreateDummyTweetDTOs(3);
			tweetRetriever.Stub(t => t.GetRecentTweets()).Return(tweets);

			var viewModel = endpoint.Get(new HomepageLinkModel());

			viewModel.ShouldContain(tweets);
		}

		private IEnumerable<TweetDTO> CreateDummyTweetDTOs(int amount)
		{
			for (int i = 0; i < amount; i++)
			{
				yield return new TweetDTO
				             	{
									Date = DateTime.Now.AddDays(-i).ToShortDateString(),
									Text = "This is a twwwwweeet " + i
				             	};
			}
		}

		[Test]
		public void Get_ShouldGrabCurrentlyReadingBooks_AndPutThemOnViewModel()
		{
			blogRetriever.ReturnEmptyCollectionSoDoesntBreakTest();

			var books = BookTestingHelper.GetSomeCurrentlyReadingBooks(3);

			bookRetriever.Stub(r => r.GetCurrentlyReading()).Return(books);

			var viewModel = endpoint.Get(new HomepageLinkModel());

			viewModel.ShouldContain(books);
		}
	}

	// TODO - move this?
	public static class IBlogPostRetrieverTestExtensions
	{
		public static void ReturnEmptyCollectionSoDoesntBreakTest(this IBlogPostsRetriever retriever)
		{
			retriever.Stub(r => r.GetRecentBlogEntries()).Return(Enumerable.Empty<BlogPostDTO>());
		}
	}
}