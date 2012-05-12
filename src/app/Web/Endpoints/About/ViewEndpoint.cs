using Model.About;
using Web.Endpoints.About.LinkModels;
using Web.Endpoints.About.ViewModels;

namespace Web.Endpoints.About
{
	public class ViewEndpoint
	{
		private readonly IAboutInfoRetriever retriever;

		public ViewEndpoint(IAboutInfoRetriever retriever)
		{
			this.retriever = retriever;
		}

		public AboutViewModel Get(AboutLinkModel linkModel)
		{
			var info = retriever.GetAboutInfo();

			return new AboutViewModel
			       	{
			       		AboutText_Html  = info.AboutText,
			       		ThingsILikeUrls = info.ThingsILikeUrls
			       	};
		}
	}
}