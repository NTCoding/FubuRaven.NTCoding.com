using System.Collections.Generic;
using Model.Services.dtos;

namespace Model.Services
{
	public interface IBlogPostRetriever
	{
		IEnumerable<BlogPostDTO> GetRecentBlogEntries();
	}
}