using System;
using System.Linq;
using Model.Services;
using Model.Services.dtos;
using Raven.Client;
using Web.Endpoints.SiteManagement.Genre.CreateGenreModels;

namespace Web.Endpoints.SiteManagement.Genre
{
	public class CreateEndpoint
	{
		private readonly IGenreCreater creater;

		public CreateEndpoint(IGenreCreater creater)
		{
			this.creater = creater;
		}

		public String Post(CreateGenreInputModel model)
		{
			var id = creater.Create(new CreateGenreDto {Name = model.Name});

			return id;
		}
	}
}