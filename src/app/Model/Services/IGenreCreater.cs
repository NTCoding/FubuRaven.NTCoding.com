using System;
using Model.Services.dtos;

namespace Model.Services
{
	public interface IGenreCreater
	{
		String Create(CreateGenreDto dto);
	}
}