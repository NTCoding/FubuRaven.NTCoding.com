using System.Web;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using Web.Endpoints;

namespace Web.Configuration.Behaviours.Output
{
	// TODO - use IHttpWriter and add some tests
	public class ImageOutputBehaviour : BasicBehavior
	{
		private readonly IFubuRequest request;

		public ImageOutputBehaviour(IFubuRequest request) : base(PartialBehavior.Ignored)
		{
			this.request = request;
		}

		protected override DoNext performInvoke()
		{
			var model = request.Get<ImageModel>();

			var response = HttpContext.Current.Response;
			response.BinaryWrite(model.Data);
			response.ContentType = model.ContentType;

			return DoNext.Continue;
		}
	}
}