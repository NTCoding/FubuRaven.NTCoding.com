using System.Collections.Generic;
using System.Web;

namespace Model.Services
{
	public interface IBookCreater
	{
		Book Create(CreateBookDto createBookDto);
	}
}