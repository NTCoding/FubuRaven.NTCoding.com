using System;
using System.Linq;
using FubuMVC.Core;
using FubuMVC.Core.Registration;
using FubuMVC.Core.UI.Configuration;
using FubuMVC.Spark;
using FubuMVC.Validation;
using FubuValidation;
using FubuValidation.Fields;
using HtmlTags;

namespace Web.Configuration
{
    public class NTCodingFubuRegistry : FubuRegistry
    {
        public NTCodingFubuRegistry()
        {
            IncludeDiagnostics(true);

            Routes
				.IgnoreNamespaceText("Endpoints")
				.IgnoreClassSuffix("Endpoint")
				.IgnoreMethodsNamed("get")
				.IgnoreMethodsNamed("post")
				.ConstrainToHttpMethod(x => x.Method.Name.Equals("Get"), "Get")
				.ConstrainToHttpMethod(x => x.Method.Name.Equals("Post"), "Post")
                .RootAtAssemblyNamespace();

        	Actions
        		.IncludeTypes(t => t.Namespace.Contains("Web.Endpoints") && t.Name.EndsWith("Endpoint"));

			Views
        		.TryToAttach(x => x.by_ViewModel())
				.RegisterActionLessViews(t => t.ViewModelType == typeof(Notification));

			this.Validation(validation =>
								{
									validation
										.Actions
										.Include(call => call.HasInput && call.InputType().Name.Contains("Input"));

									validation
										.Failures
										.If(f => f.InputType() != null && f.InputType().Name.Contains("Input"))
										.TransferBy<HandlerModelDescriptor>();
								});



        	this.UseSpark();
        }
    }

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