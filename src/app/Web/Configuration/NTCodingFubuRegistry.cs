using FubuMVC.Core;
using FubuMVC.Spark;

namespace Web.Configuration
{
    public class NTCodingFubuRegistry : FubuRegistry
    {
        public NTCodingFubuRegistry()
        {
            IncludeDiagnostics(true);

            Routes
				.IgnoreNamespaceText("EndPoints")
				.IgnoreClassSuffix("EndPoint")
				.IgnoreMethodsNamed("get")
				.IgnoreMethodsNamed("post")
				.ConstrainToHttpMethod(x => x.Method.Name.Equals("Get"), "Get")
				.ConstrainToHttpMethod(x => x.Method.Name.Equals("Post"), "Post")
                .RootAtAssemblyNamespace();

        	Actions
        		.IncludeTypes(t => t.Namespace.Contains("Web.EndPoints") && t.Name.EndsWith("EndPoint"));


        	Views
        		.TryToAttach(x => x.by_ViewModel());

        	this.UseSpark();
        }
    }
}