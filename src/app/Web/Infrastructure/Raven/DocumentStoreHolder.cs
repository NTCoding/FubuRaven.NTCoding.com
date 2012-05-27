using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;

namespace Web.Infrastructure.Raven
{
	public class DocumentStoreHolder
	{
		private static IDocumentStore _documentStore;

		public static IDocumentStore DocumentStore
		{
			get { return (_documentStore ?? (_documentStore = CreateDocumentStore())); }
		}

		private static IDocumentStore CreateDocumentStore()
		{
			var store = GetStore();

			store.Initialize();

			store.Conventions.MaxNumberOfRequestsPerSession = 500;

			return store;
		}

		private static DocumentStore GetStore()
		{
			
			if (bool.Parse(WebConfigurationManager.AppSettings["IsLocal"]))
			{
				return new EmbeddableDocumentStore
				        	{
								DataDirectory = "@App_Data\\Raven",
								UseEmbeddedHttpServer = true,
				        	};
			}

			return new DocumentStore
			       	{
			       		ConnectionStringName = "RavenDB",
			       		ApiKey = WebConfigurationManager.AppSettings["RavenApiKey"],
			       	};
		}
	}
}