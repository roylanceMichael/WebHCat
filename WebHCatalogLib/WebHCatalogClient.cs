namespace Roylance.WebHCatalogLib
{
	using System.Collections.Generic;

	using Newtonsoft.Json;

	using Roylance.WebHCatalogLib.Models;
	using Roylance.WebHCatalogLib.Resources;
	using System.Threading.Tasks;

	public class WebHCatalogClient : IWebHCatalogClient
	{
		private const string WebHdfsLocation = "hdfs://{0}:{1}/{2}";

		private readonly ServerInfoModel hdfsInfo;

		private readonly UrlBuilder urlBuilder;

		private readonly WebClientBuilder webClientBuilder;

		public WebHCatalogClient(ServerInfoModel hCatalogInfo, ServerInfoModel hdfsInfo, CredentialModel credentials, bool useKerberos = false)
		{
			hCatalogInfo.CheckWhetherArgumentIsNull("hCatalogInfo");
			hdfsInfo.CheckWhetherArgumentIsNull("hdfsInfo");
			credentials.CheckWhetherArgumentIsNull("networkCredential");

			this.hdfsInfo = hdfsInfo;
			this.urlBuilder = new UrlBuilder(hCatalogInfo, credentials, useKerberos);
			this.webClientBuilder = new WebClientBuilder(credentials, useKerberos);
		}

		public Task<GetDatabasesModel> GetDatabases()
		{
			var databaseUrl = this.urlBuilder.GetDatabasesUrl();

			return Task<GetDatabasesModel>.Factory.StartNew(
				() =>
				{
					using (var client = this.webClientBuilder.BuildWebClient())
					{
						return client.GetJson<GetDatabasesModel>(databaseUrl);
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

		public Task<bool> CreateDatabase(string database, string comment, string location)
		{
			var createDatabaseUrl = this.urlBuilder.CreateDatabaseUrl(database);

			var jsonDictionary = new Dictionary<string, string>();

			jsonDictionary["location"] = WebHdfsLocation.FormatTemplate(this.hdfsInfo.Server, this.hdfsInfo.Port, location);

			if (!string.IsNullOrWhiteSpace(comment))
			{
				jsonDictionary["comment"] = comment;
			}

			var inputJson = JsonConvert.SerializeObject(jsonDictionary);

			return Task<bool>.Factory.StartNew(
				() =>
				{
					using (var restService = this.webClientBuilder.BuildWebClient())
					{
						var response = restService.PutJson<Dictionary<string, string>>(createDatabaseUrl, inputJson);

						// project response to true/false
						return response.ContainsKey("database") && response.ContainsValue(database);
					}
				});
		}

		public Task<GetTablesModel> GetTables(string database)
		{
			var getTablesUrl = this.urlBuilder.GetTablesUrl(database);
			return Task<GetTablesModel>.Factory.StartNew(
				() =>
				{
					using (var restService = this.webClientBuilder.BuildWebClient())
					{
						return restService.GetJson<GetTablesModel>(getTablesUrl);
					}
				});
		}
	}
}
