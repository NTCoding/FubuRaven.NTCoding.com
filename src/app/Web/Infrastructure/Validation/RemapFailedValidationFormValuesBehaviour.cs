using System;
using System.Linq;
using System.Reflection;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using Enumerable = System.Linq.Enumerable;

namespace Web.Infrastructure.Validation
{
	public class RemapFailedValidationFormValuesBehaviour<TInputModel> : IActionBehavior where TInputModel : class
	{
		private readonly IFubuRequest request;

		public RemapFailedValidationFormValuesBehaviour(IFubuRequest request)
		{
			this.request = request;
		}

		public IActionBehavior InnerBehavior { get; set; }

		public void Invoke()
		{
			ValidatedInputModel<TInputModel> validatedInputModel = null;
			try
			{
				validatedInputModel = request.Get<ValidatedInputModel<TInputModel>>();
			}
			catch (FubuException)
			{
				InnerBehavior.Invoke();
				return;
			}

			if (validatedInputModel.FailedValidation)
			{
				var viewModel = request.Get(GetViewModelType());

				MapProperties(validatedInputModel.InputModel, viewModel);
			}

			InnerBehavior.Invoke();
		}

		// TODO - could be a convention and live elsewhere
		//        maybe put on the IActionFinder
		private Type GetViewModelType()
		{
			return
				Assembly
					.GetAssembly(typeof(TInputModel))
					.GetTypes()
					.Where(t => t.IsSubclassOf(typeof(TInputModel))).First();
		}

		private void MapProperties(object from, object to)
		{
			foreach (var pi in from.GetType().GetProperties())
			{
				var prop = to.GetType().GetProperties().SingleOrDefault(ti => ti.Name == pi.Name);
				if (prop != null)
				{
					prop.SetValue(to, pi.GetValue(from, new object[] { }), new object[] { });
				}
			}
		}

		

		public void InvokePartial()
		{
			Invoke();
		}
	}
}