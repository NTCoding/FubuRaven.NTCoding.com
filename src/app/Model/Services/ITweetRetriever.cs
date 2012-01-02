using System.Collections.Generic;
using Model.Services.dtos;

namespace Model.Services
{
	public interface ITweetRetriever
	{
		IEnumerable<TweetDTO> GetRecentTweets();
	}
}