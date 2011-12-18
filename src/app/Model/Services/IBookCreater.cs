using System;
using System.Collections.Generic;
using System.Web;
using Model.Services.dtos;

namespace Model.Services
{
	public interface IBookCreater
	{
		// TODO - should only return the Id
		Book Create(CreateBookDto createBookDto);
	}
}