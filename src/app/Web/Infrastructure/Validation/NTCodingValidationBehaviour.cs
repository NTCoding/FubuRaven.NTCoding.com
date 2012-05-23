using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using FubuValidation;

namespace Web.Infrastructure.Validation
{
	public class NTCodingValidationBehaviour<T> : IActionBehavior where T : class
	{
		private readonly IFubuRequest request;
		private readonly IValidator validator;
		private readonly IActionFinder actionFinder;
		private readonly IPartialFactory factory;

		public NTCodingValidationBehaviour(IFubuRequest request, IValidator validator, IActionFinder actionFinder, IPartialFactory factory)
		{
			this.request      = request;
			this.factory      = factory;
			this.validator    = validator;
			this.actionFinder = actionFinder;
		}

		public IActionBehavior InnerBehavior { get; set; }

		public void Invoke()
		{
			var inputModel = request.Get<T>();

			var notification = validator.Validate(inputModel);

			if (notification.IsValid())
				SetContextAndContinueChain(inputModel);
			else
				SetContextAndRedirectToGet(inputModel, notification);
		}

		private void SetContextAndContinueChain(T inputModel)
		{
			request.Set(new ValidatedInputModel<T>(inputModel) { FailedValidation = false });

			InnerBehavior.Invoke();
		}

		private void SetContextAndRedirectToGet(T inputModel, Notification notification)
		{
			request.Set(notification);
			request.Set(new ValidatedInputModel<T>(inputModel) { FailedValidation = true });

			factory.BuildPartial(actionFinder.GetRequestModelTypeFor(inputModel)).InvokePartial();
		}
		
		public void InvokePartial()
		{
			Invoke();
		}
	}
}