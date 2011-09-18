using FubuMVC.Core;
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

        	HtmlConvention(x =>
        	                    x.Editors
        						  .If(e => e.Accessor.Name.EndsWith("_BigText"))
        	                      .BuildBy(er => new HtmlTag("textarea"))
						);

        	HtmlConvention(x =>
        	               x.Labels
        	               	.If(e => e.Accessor.Name.EndsWith("_BigText"))
        	               	.BuildBy(er => new HtmlTag("label").Text(er.Accessor.Name.Replace("_BigText", "")))
						);


        	this.UseSpark();
        }
    }
}