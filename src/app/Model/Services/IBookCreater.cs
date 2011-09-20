using System.Collections.Generic;
using System.Web;

namespace Model.Services
{
	public interface IBookCreater
	{
		void Create(string title, IEnumerable<string> authors, string description, string genre, byte[] image, BookStatus status);
	}
}