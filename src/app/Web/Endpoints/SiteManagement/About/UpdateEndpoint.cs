using FubuMVC.Core.Continuations;
using Model.About;
using Web.Endpoints.About;
using Web.Endpoints.About.LinkModels;

namespace Web.Endpoints.SiteManagement.About
{
	public class UpdateEndpoint
	{
		private readonly IAboutInfoUpdater updater;

		public UpdateEndpoint(IAboutInfoUpdater updater)
		{
			this.updater = updater;
		}

		public FubuContinuation Update(AboutUpdateModel model)
		{
			var dto = new AboutInfoDto {AboutText = model.AboutText, ThingsILikeUrls = model.ThingsILikeUrls};
			updater.Update(dto);

			return FubuContinuation.RedirectTo<ViewEndpoint>(x => x.Get(new AboutLinkModel()));
		}
	}
}