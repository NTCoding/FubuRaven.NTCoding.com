using System.Web;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using Web.Endpoints;

namespace Web.Configuration.Behaviours.Output
{
	public class ImageOutputBehaviour : BasicBehavior
	{
		private readonly IOutputWriter writer;
		private readonly IFubuRequest request;

		public ImageOutputBehaviour(IOutputWriter writer, IFubuRequest request) : base(PartialBehavior.Ignored)
		{
			this.writer = writer;
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