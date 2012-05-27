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
			var store = new DocumentStore
			            	{
								ConnectionStringName = "RavenDB",
								ApiKey = WebConfigurationManager.AppSettings["RavenApiKey"],
							}.Initialize();

			store.Conventions.MaxNumberOfRequestsPerSession = 500;

			return store;
		}
	}
}