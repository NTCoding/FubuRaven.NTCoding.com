using System;
using System.Linq;
using FubuMVC.Core.Registration;

namespace Web.Infrastructure
{
	public class BehaviourGraphActionFinder : IActionFinder
	{
		private readonly BehaviorGraph graph;

		public BehaviourGraphActionFinder(BehaviorGraph graph)
		{
			this.graph = graph;
		}

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