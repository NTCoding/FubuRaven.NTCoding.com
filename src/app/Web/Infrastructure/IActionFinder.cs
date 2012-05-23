using System;

namespace Web.Infrastructure
{
	public interface IActionFinder
	{
		Type GetRequestModelTypeFor<T>(T inputModel);
	}
}