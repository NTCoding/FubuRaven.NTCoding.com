using System;
using System.Collections.Generic;

namespace Model.Services
{
	public interface IGenreRetriever
	{
		IDictionary<String, String> GetAllOrderedByName();
		
		bool CanFindGenreWith(string id);
	}
}