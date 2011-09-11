using System.Web.Routing;
using FubuMVC.Core;
using StructureMap;
using Web.Configuration;
using BootstrappingExtensions = FubuMVC.StructureMap.BootstrappingExtensions;

// You can remove the reference to WebActivator by calling the Start() method from your Global.asax Application_Start
[assembly: WebActivator.PreApplicationStartMethod(typeof(AppStart), "Start", callAfterGlobalAppStart: true)]

namespace Web.Configuration
{
    public static class AppStart
    {
        public static void Start()
        {
            // FubuApplication "guides" the bootstrapping of the FubuMVC
            // application
            BootstrappingExtensions.StructureMap(FubuApplication.For<NTCodingFubuRegistry>(), new Container())
                .Bootstrap(RouteTable.Routes);
        }
    }
}