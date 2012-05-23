using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Runtime;
using FubuValidation;
using Model.About;
using Web.Endpoints.About;
using Web.Endpoints.About.LinkModels;
using Web.Infrastructure.Validation;
using Web.Utilities;

namespace Web.Endpoints.SiteManagement.About
{
	public class UpdateEndpoint
	{
		private readonly IAboutInfoUpdater updater;
		private readonly IAboutInfoRetriever retriever;
		private IFubuRequest request;

		public UpdateEndpoint(IAboutInfoUpdater updater, IAboutInfoRetriever retriever, IFubuRequest request = null)
		{
			this.updater = updater;
			this.request = request;
			this.retriever = retriever;
		}

		public AboutViewModel Get(AboutRequestModel aboutRequestModel)
		{
			var n = request.Get<Notification>();

			var af = retriever.GetAboutInfo();

			return new AboutViewModel
			       	{
			       		AboutText_BigText = af.AboutText,
						ThingsILikeUrls = af.ThingsILikeUrls.ToStringWrappers().ToList()
			       	};
		}

		public FubuContinuation Post(AboutUpdateModel model)
		{
			var dto = new AboutInfoDto {AboutText = model.AboutText_BigText, ThingsILikeUrls = model.ThingsILikeUrls.ToStrings()};
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
		public string AboutText_BigText { get; set; }

		[DenyPrefix(Prefix = "http://")]
		public IList<StringWrapper> ThingsILikeUrls { get; set; }
	}
}