using System;
using System.Collections.Generic;

namespace Model.Services
{
	// TODO - could there be a generic implementation for retrievers? Similar to generic repository?
	public interface IGenreRetriever
	{
		IDictionary<String, String> GetAll();
		
		bool CanFindGenreWith(string id);
	}
}