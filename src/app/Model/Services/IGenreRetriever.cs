using System.Collections.Generic;

namespace Model.Services
{
	public interface IGenreRetriever
	{
		IDictionary<string, string> GetAllOrderedByName();
	}
}