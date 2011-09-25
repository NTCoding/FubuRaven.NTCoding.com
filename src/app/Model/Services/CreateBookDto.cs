using System.Collections.Generic;

namespace Model.Services
{
	public class CreateBookDto
	{
		public string Title { get; set; }

		public IEnumerable<string> Authors { get; set; }

		public string Description { get; set; }

		public string Genre { get; set; }

		public byte[] Image { get; set; }

		public BookStatus Status { get; set; }
	}
}