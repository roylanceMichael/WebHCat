namespace Roylance.WebHCatalogLib
{
	using System.Threading.Tasks;

	using Roylance.WebHCatalogLib.Models;

	public interface IWebHCatalogClient
	{
		Task<GetDatabasesModel> GetDatabases();

		Task<bool> CreateDatabase(string database, string comment, string location);

		Task<bool> DeleteDatabase(string database);

		Task<GetTablesModel> GetTables(string database);
	}
}
