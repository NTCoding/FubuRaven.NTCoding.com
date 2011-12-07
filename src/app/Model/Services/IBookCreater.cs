using System.Collections.Generic;
using System.Web;
using Model.Services.dtos;

namespace Model.Services
{
	public interface IBookCreater
	{
		Book Create(CreateBookDto createBookDto);
	}
}