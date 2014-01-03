namespace Roylance.WebHCatalogLib
{
	using System.Collections.Generic;

	using Roylance.WebHCatalogLib.Models;
	using Roylance.WebHCatalogLib.Resources;
	using System.Threading.Tasks;

	public class WebHCatalogClient : IWebHCatalogClient
	{
		private const string WebHdfsLocation = "hdfs://{0}:{1}/{2}";

		private readonly ServerInformation hdfsInformation;

		private readonly UrlBuilder urlBuilder;

		private readonly WebClientBuilder webClientBuilder;

		public WebHCatalogClient(ServerInformation hCatalogInformation, ServerInformation hdfsInformation, Credential credentials, bool useKerberos = false)
		{
			hCatalogInformation.CheckWhetherArgumentIsNull("hCatalogInformation");
			hdfsInformation.CheckWhetherArgumentIsNull("hdfsInformation");
			credentials.CheckWhetherArgumentIsNull("networkCredential");

			this.hdfsInformation = hdfsInformation;
			this.urlBuilder = new UrlBuilder(hCatalogInformation, credentials, useKerberos);
			this.webClientBuilder = new WebClientBuilder(credentials, useKerberos);
		}

		public Task<GetDatabasesResponse> GetDatabases()
		{
			var databaseUrl = this.urlBuilder.GetDatabasesUrl();

			return Task<GetDatabasesResponse>.Factory.StartNew(
				() =>
				{
					using (var client = this.webClientBuilder.BuildWebClient())
					{
						return client.GetJson<GetDatabasesResponse>(databaseUrl);
					}
				});
		}

		public Task<bool> DeleteDatabase(string database)
		{
			var deleteDatabaseUrl = this.urlBuilder.DeleteDatabaseUrl(database);

			return Task<bool>.Factory.StartNew(
				() =>
				{
					using (var restfulService = this.webClientBuilder.BuildWebClient())
					{
						var result = restfulService.DeleteJson<Dictionary<string, string>>(deleteDatabaseUrl, string.Empty);
						return result.ContainsKey("database") && result.ContainsValue(database);
					}
				});
		}

		public Task<bool> CreateDatabase(CreateDatabaseRequest createDatabaseRequest)
		{
			createDatabaseRequest.CheckWhetherArgumentIsNull("createDatabaseRequest");

			var createDatabaseUrl = this.urlBuilder.CreateDatabaseUrl(createDatabaseRequest.Database);

			// TODO: think of a more elegant way to do this
			createDatabaseRequest.Location = WebHdfsLocation.FormatTemplate(
				this.hdfsInformation.Server,
				this.hdfsInformation.Port,
				createDatabaseRequest.Location);

			return Task<bool>.Factory.StartNew(
				() =>
				{
					using (var restService = this.webClientBuilder.BuildWebClient())
					{
						var response = restService.PutJson<Dictionary<string, string>>(createDatabaseUrl, createDatabaseRequest.ToJson());

						// project response to true/false
						return response.ContainsKey("database") && response.ContainsValue(createDatabaseRequest.Database);
					}
				});
		}

		public Task<GetTablesResponse> GetTables(string database)
		{
			database.CheckWhetherArgumentIsNull("database");

			var getTablesUrl = this.urlBuilder.GetTablesUrl(database);
			return Task<GetTablesResponse>.Factory.StartNew(
				() =>
				{
					using (var restService = this.webClientBuilder.BuildWebClient())
					{
						return restService.GetJson<GetTablesResponse>(getTablesUrl);
					}
				});
		}

		public Task<bool> CreateTable(CreateTableRequest createTableRequest)
		{
			createTableRequest.CheckWhetherArgumentIsNull("createTableRequest");

			var createTablesUrl = this.urlBuilder.CreateTableUrl(createTableRequest.Database, createTableRequest.Table);

			return Task<bool>.Factory.StartNew(
				() =>
				{
					using (var restService = this.webClientBuilder.BuildWebClient())
					{
						var requestJson = createTableRequest.ToJson();
						var dictionaryResult = restService.PutJson<Dictionary<string, string>>(
							createTablesUrl,
							requestJson);

						return dictionaryResult.ContainsKey("table") &&
							dictionaryResult.ContainsValue(createTableRequest.Table) &&
							dictionaryResult.ContainsKey("database") &&
							dictionaryResult.ContainsValue(createTableRequest.Database);
					}
				});
		}

		public Task<bool> DeleteTable(string database, string table)
		{
			database.CheckWhetherArgumentIsNull("database");
			table.CheckWhetherArgumentIsNull("table");

			var deleteTableUrl = this.urlBuilder.DeleteTableUrl(database, table);

			return Task<bool>.Factory.StartNew(
				() =>
					{
						using (var restService = this.webClientBuilder.BuildWebClient())
						{
							var dictionaryResult = restService.DeleteJson<Dictionary<string, string>>(deleteTableUrl, string.Empty);
							return dictionaryResult.ContainsKey("table") && dictionaryResult.ContainsValue(table)
							       && dictionaryResult.ContainsKey("database") && dictionaryResult.ContainsValue(database);
						}
					});
		}
	}
}
