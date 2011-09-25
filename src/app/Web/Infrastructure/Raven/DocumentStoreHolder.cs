using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
			var store = new EmbeddableDocumentStore
			            	{
								DataDirectory = "@App_Data\\Raven",
								UseEmbeddedHttpServer = true,
							}.Initialize();

			return store;
		}
	}
}