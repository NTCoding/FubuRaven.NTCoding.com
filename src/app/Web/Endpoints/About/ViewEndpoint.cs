using Model.About;

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
			       		AboutText       = info.AboutText,
			       		ThingsILikeUrls = info.ThingsILikeUrls
			       	};
		}
	}
}