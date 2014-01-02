namespace Roylance.WebHCatalogLib
{
	using System.Threading.Tasks;

	using Roylance.WebHCatalogLib.Models;

	public interface IWebHCatalogClient
	{
		Task<GetDatabasesResponse> GetDatabases();

		Task<bool> CreateDatabase(CreateDatabaseRequest createDatabaseRequest);

		Task<bool> DeleteDatabase(string database);

		Task<GetTablesResponse> GetTables(string database);

		Task<bool> CreateTable(CreateTableRequest createTableRequest);
	}
}
