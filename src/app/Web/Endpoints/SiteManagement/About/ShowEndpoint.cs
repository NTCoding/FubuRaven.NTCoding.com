using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client;

namespace Web.Endpoints.SiteManagement.About
{
	public class ShowEndpoint
	{
		private readonly IDocumentSession session;

		public ShowEndpoint(IDocumentSession session)
		{
			this.session = session;
		}

		public CarsViewModel Get(CarsRequestModel requestModel)
		{
			var cars = GetCars(requestModel);

			var model = new CarsViewModel();

			Map(cars, model);

			return model;
		}

		private IQueryable<Car> GetCars(CarsRequestModel requestModel)
		{
			return session
				.Query<Car>()
				.Where(c => c.Price < requestModel.MinPrice)
				.Where(c => c.Price > requestModel.MaxPrice)
				.Take(requestModel.PageSize);
		}

		private void Map(IQueryable<Car> cars, CarsViewModel model)
		{
			var carDtos = new List<ShowCarDto>();
			foreach (var car in cars)
			{
				var dto = new ShowCarDto
				{
					Model         = car.Model,
					Manufacturer  = car.Manufacturer,
					Price         = car.Price,
					NumberOfDoors = car.NumberOfDoors
				};

				carDtos.Add(dto);
			}

			model.Cars = carDtos;
		}
	}

	public class CarsRequestModel
	{
		public decimal MinPrice { get; set; }

		public decimal MaxPrice { get; set; }

		public int PageSize { get; set; }
	}

	public class CarsViewModel
	{
		public IEnumerable<ShowCarDto> Cars { get; set; }
	}

	public class ShowCarDto
	{
		public string Model { get; set; }

		public string Manufacturer { get; set; }

		public decimal Price { get; set; }

		public int NumberOfDoors { get; set; }
	}

	public class Car
	{
		public decimal Price { get; set; }

		public string Model { get; set; }

		public string Manufacturer { get; set; }

		public int NumberOfDoors { get; set; }
	}
}