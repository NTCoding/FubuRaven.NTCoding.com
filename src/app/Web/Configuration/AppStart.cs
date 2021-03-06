﻿using System;
using System.ComponentModel;
using System.Web.Routing;
using FubuMVC.Core;
using FubuMVC.Core.Continuations;
using FubuMVC.Validation;
using FubuValidation;
using Model.About;
using Model.Services;
using Raven.Client;
using Raven.Client.Embedded;
using StructureMap;
using Web.Configuration;
using Web.Infrastructure;
using Web.Infrastructure.Authxx;
using Web.Infrastructure.Raven;
using Web.Utilities;
using BootstrappingExtensions = FubuMVC.StructureMap.BootstrappingExtensions;
using Container = StructureMap.Container;

// You can remove the reference to WebActivator by calling the Start() method from your Global.asax Application_Start
[assembly: WebActivator.PreApplicationStartMethod(typeof(AppStart), "Start")]

namespace Web.Configuration
{
    public static class AppStart
    {
        public static void Start()
        {
			// TODO - move declarations into a registry
			var container = new Container(x =>
			{

				x.For<IDocumentSession>()
					.HttpContextScoped()
					.Use(c => DocumentStoreHolder.DocumentStore.OpenSession());

				x.For<IHomepageContentProvider>().Use<HomepageContentProvider>();

				x.For<IBookCreater>().Use<BookCreater>();

				x.For<ImagePreparer>().Use<SimpleImagePreparer>();

				x.For<IBookUpdater>().Use<RavenDbBookUpdater>();

				x.For<IGenreRetriever>().Use<RavenDbGenreRetriever>();

				x.For<IGenreCreater>().Use<RavenDbGenreCreater>();

				x.For<IBookRetriever>().Use<RavenDbBookRetriever>();

				x.For<IBlogPostsRetriever>().Use<BlogPostsRetriever>();

				x.For<ITweetRetriever>().Use<AsyncTimeoutTweetRetriever>();

				x.For<IAboutInfoRetriever>().Use<RavenAboutInfoRetriever>();

				x.For<IAboutInfoUpdater>().Use<RavenAboutInfoUpdater>();

				x.For<IDoorStaff>().Use<HardBastardsDoorStaff>();

				x.For<IValidator>().Use(Validator.BasicValidator);

				x.For<IValidationContinuationHandler>().Use<ValidationContinuationHandler>();

				x.For<IContinuationDirector>().Use<ContinuationHandler>();

				x.For<IActionFinder>().Use<BehaviourGraphActionFinder>();
			});

        	BootstrappingExtensions.StructureMap(FubuApplication.For<NTCodingFubuRegistry>(), container)
                .Bootstrap();
        }
    }
}