using DataImporter;
using Raven.Client;

namespace Web.Endpoints
{
	public class ImportEndpoint
	{
		private readonly IDocumentSession session;

		public ImportEndpoint(IDocumentSession session)
		{
			this.session = session;
		}

		public ImportViewModel Get(ImportLinkModel model)
		{
			new RavenDbImporter().Import(session);

			return new ImportViewModel();
		}
	}

	public class ImportLinkModel
	{
	}

	public class ImportViewModel
	{
	}
}