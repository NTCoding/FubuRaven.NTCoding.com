using System.Collections.Generic;

namespace Model.Services
{
	public interface IBookCreater
	{
		void Create(string title, IEnumerable<string> authors, string description, string genre, byte[] image, string status);
	}
}