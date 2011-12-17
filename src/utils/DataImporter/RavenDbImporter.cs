using System;
using System.IO;
using System.Reflection;
using Raven.Client;
using Raven.Client.Embedded;

namespace DataImporter
{
	public class RavenDbImporter
	{
		public void Import(IDocumentSession session)
		{
			new Persister(new SqlImporter(), session).Persist();
		}
	}
}