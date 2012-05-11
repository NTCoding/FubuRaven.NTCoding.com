using System;
using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core.Continuations;
using Model.About;
using Web.Endpoints.About;
using Web.Endpoints.About.LinkModels;

namespace Web.Endpoints.SiteManagement.About
{
	public class UpdateEndpoint
	{
		private readonly IAboutInfoUpdater updater;
		private readonly IAboutInfoRetriever retriever;

		public UpdateEndpoint(IAboutInfoUpdater updater, IAboutInfoRetriever retriever)
		{
			this.updater = updater;
			this.retriever = retriever;
		}

		public AboutViewModel Get(AboutRequestModel aboutRequestModel)
		{
			var af = retriever.GetAboutInfo();

			return new AboutViewModel
			       	{
			       		AboutText = af.AboutText,
						ThingsILikeUrls = af.ThingsILikeUrls.ToList()
			       	};
		}

		public FubuContinuation Post(AboutUpdateModel model)
		{
			var dto = new AboutInfoDto {AboutText = model.AboutText, ThingsILikeUrls = model.ThingsILikeUrls};
			updater.Update(dto);

			return FubuContinuation.RedirectTo<ViewEndpoint>(x => x.Get(new AboutLinkModel()));
		}
	}

	public class AboutRequestModel
	{
	}

	public class AboutViewModel : AboutUpdateModel
	{
		
	}

	public class AboutUpdateModel
	{
		public string AboutText { get; set; }

		public List<string> ThingsILikeUrls { get; set; }
	}
}