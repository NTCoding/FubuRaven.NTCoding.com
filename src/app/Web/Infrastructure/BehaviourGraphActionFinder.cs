using System;
using System.Linq;
using FubuMVC.Core.Registration;

namespace Web.Infrastructure
{
	public class BehaviourGraphActionFinder : IActionFinder
	{
		private BehaviorGraph graph;

		public Type GetRequestModelTypeFor<T>(T inputModel)
		{
			var targetNamespace = inputModel.GetType().Namespace;

			var getCall = graph
				.Behaviors
				.Where(chain => chain.FirstCall() != null && chain.FirstCall().HandlerType.Namespace == targetNamespace
				                && chain.Route.AllowedHttpMethods.Contains("GET"))
				.Select(chain => chain.FirstCall())
				.FirstOrDefault();

			return getCall.InputType();
		}
	}
}