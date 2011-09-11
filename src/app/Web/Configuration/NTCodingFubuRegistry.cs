using FubuMVC.Core;

namespace Web.Configuration
{
    public class NTCodingFubuRegistry : FubuRegistry
    {
        public NTCodingFubuRegistry()
        {
            // This line turns on the basic diagnostics and request tracing
            IncludeDiagnostics(true);

            // Policies
            Routes
				.IgnoreNamespaceText("EndPoints")
                .IgnoreMethodSuffix("Html")
				.IgnoreClassSuffix("EndPoint")
				.IgnoreMethodsNamed("get")
				.IgnoreMethodsNamed("post")
				.ConstrainToHttpMethod(x => x.Method.Name.Equals("Get"), "Get")
				.ConstrainToHttpMethod(x => x.Method.Name.Equals("Post"), "Post")
                .RootAtAssemblyNamespace();

        	Actions
        		.IncludeTypes(t => t.Namespace.Contains("Web.EndPoints") && t.Name.EndsWith("EndPoint"));


            // Match views to action methods by matching
            // on model type, view name, and namespace
            Views.TryToAttachWithDefaultConventions();
        }
    }
}