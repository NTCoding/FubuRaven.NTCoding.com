using System;
using System.Linq;
using FubuMVC.Core.Registration;
using FubuMVC.Validation;

namespace Web.Configuration
{
	public class HandlerModelDescriptor : IFubuContinuationModelDescriptor
	{
		private readonly BehaviorGraph _graph;

		public HandlerModelDescriptor(BehaviorGraph graph)
		{
			_graph = graph;
		}

		public Type DescribeModelFor(ValidationFailure context)
		{
			var targetNamespace = context.Target.HandlerType.Namespace;
			var getCall = _graph
				.Behaviors
				.Where(chain => chain.FirstCall() != null && chain.FirstCall().HandlerType.Namespace == targetNamespace
				                && chain.Route.AllowedHttpMethods.Contains("GET"))
				.Select(chain => chain.FirstCall())
				.FirstOrDefault();

			if (getCall == null)
			{
				return null;
			}

			return getCall.InputType();
		}
	}
}