using System;
using FubuMVC.Core;
using FubuMVC.Core.UI.Configuration;
using FubuMVC.Spark;

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
        		.TryToAttach(x => x.by_ViewModel());

        	this.UseSpark();
        }
    }
}