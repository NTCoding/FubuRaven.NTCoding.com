using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Model.Services;
using Model.Services.dtos;

namespace Web.Utilities
{
	public class AsyncTimeoutTweetRetriever : ITweetRetriever
	{
		public IEnumerable<TweetDTO> GetRecentTweets()
		{
			return AsyncHelper.DoTimedOperation(GetRecentNTCodingTweets, 10, Enumerable.Empty<TweetDTO>());
		}

		private IEnumerable<TweetDTO> GetRecentNTCodingTweets()
		{
			var tweetFeed = XDocument.Load("http://api.twitter.com/1/statuses/user_timeline/131558161.rss");
			var tweets = tweetFeed.Descendants("item");
			var mostRecentTweets = tweets.Take(3);

			return mostRecentTweets.Select(t => new TweetDTO
			                                    	{
			                                    		Date = Convert.ToDateTime(t.Element("pubDate").Value).ToShortDateString(),
			                                    		Text = t.Element("description").Value.Substring(9)
			                                    	});
		}
	}
}