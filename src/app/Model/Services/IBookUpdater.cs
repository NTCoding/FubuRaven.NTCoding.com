using Model.Services.dtos;

namespace Model.Services
{
	public interface IBookUpdater
	{
		void Update(UpdateBookDto dto);
	}
}