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
			var store = new DocumentStore
			            	{
								Url = @"https://1.ravenhq.com/databases/NTCoding-FubuRavenNTCoding",
								ApiKey = @"9cac5fef-26bc-452b-8ce1-5264eab65336",
							}.Initialize();

			store.Conventions.MaxNumberOfRequestsPerSession = 500;

			return store;
		}
	}
}