using System;

namespace Model.Services.dtos
{
	public class UpdateBookDto : CreateBookDto
	{
		public string Id { get; set; }

		public int Rating { get; set; }
	}
}