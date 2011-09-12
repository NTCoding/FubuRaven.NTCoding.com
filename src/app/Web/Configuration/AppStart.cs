using System;
using System.Web.Routing;
using FubuMVC.Core;
using Model;
using Raven.Client;
using Raven.Client.Embedded;
using StructureMap;
using Web.Configuration;
using Web.Infrastructure.Raven;
using BootstrappingExtensions = FubuMVC.StructureMap.BootstrappingExtensions;

// You can remove the reference to WebActivator by calling the Start() method from your Global.asax Application_Start
[assembly: WebActivator.PreApplicationStartMethod(typeof(AppStart), "Start", callAfterGlobalAppStart: true)]

namespace Web.Configuration
{
    public static class AppStart
    {
        public static void Start()
        {
			// TODO - move declarations into a registry
        	var container = new Container(x => {
						x.For<IDocumentSession>().Use(DocumentStoreHolder.DocumentStore.OpenSession());
        				x.For<IHomepageContentProvider>().Use<HomepageContentProvider>();
        			});

        	BootstrappingExtensions.StructureMap(FubuApplication.For<NTCodingFubuRegistry>(), container)
                .Bootstrap(RouteTable.Routes);
        }
    	
    }
}