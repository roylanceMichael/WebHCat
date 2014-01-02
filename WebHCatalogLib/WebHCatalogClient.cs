namespace Roylance.WebHCatalogLib
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	using Newtonsoft.Json;
	using Newtonsoft.Json.Serialization;

	using Roylance.WebHCatalogLib.Models;
	using Roylance.WebHCatalogLib.Resources;
	using System.Threading.Tasks;

	public class WebHCatalogClient : IWebHCatalogClient
	{
		private const string WebHdfsLocation = "hdfs://{0}:{1}/{2}";

		private const string Post = "POST";

		private const string Put = "PUT";

		private const string Delete = "DELETE";

		private readonly ServerInfoModel hdfsInfo;

		private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

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

		public async Task<GetDatabasesModel> GetDatabases()
		{
			var databaseUrl = this.urlBuilder.GetDatabasesUrl();
			var jsonResult = await this.GetJson(databaseUrl);
			return JsonConvert.DeserializeObject<GetDatabasesModel>(jsonResult, this.serializerSettings);
		}

		public async Task<bool> DeleteDatabase(string database)
		{
			var deleteDatabaseUrl = this.urlBuilder.DeleteDatabaseUrl(database);

			var jsonResult = await this.DeleteJson(deleteDatabaseUrl, string.Empty);
			var dictionaryResult = this.DeserializeIntoDictionary(jsonResult);

			return dictionaryResult.ContainsKey("database");
		}

		public async Task<bool> CreateDatabase(string database, string comment, string location)
		{
			var createDatabaseUrl = this.urlBuilder.CreateDatabaseUrl(database);

			var jsonDictionary = new Dictionary<string, string>();

			jsonDictionary["location"] = WebHdfsLocation.FormatTemplate(this.hdfsInfo.Server, this.hdfsInfo.Port, location);

			if (!string.IsNullOrWhiteSpace(comment))
			{
				jsonDictionary["comment"] = comment;
			}

			var responseString = await this.PutJson(createDatabaseUrl, JsonConvert.SerializeObject(jsonDictionary));

			var actualResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString, this.serializerSettings);

			return actualResponse.ContainsKey("database") && actualResponse.ContainsValue(database);
		}

		public async Task<GetTablesModel> GetTables(string database)
		{
			var getTablesUrl = this.urlBuilder.GetTablesUrl(database);
			var result = await this.GetJson(getTablesUrl);
			return JsonConvert.DeserializeObject<GetTablesModel>(result);
		}

		protected Task<string> DeleteJson(string url, string data)
		{
			return this.SendJson(url, data, Delete);
		}

		protected Task<string> PostJson(string url, string data)
		{
			return this.SendJson(url, data, Post);
		}

		protected Task<string> PutJson(string url, string data)
		{
			return this.SendJson(url, data, Put);
		}

		protected Task<string> SendJson(string url, string data, string method)
		{
			url.CheckWhetherArgumentIsNull("url");
			data.CheckWhetherArgumentIsNull("data");
			method.CheckWhetherArgumentIsNull("method");

			return Task<string>.Factory.StartNew(
				() =>
				{
					var bytes = Encoding.Default.GetBytes(data);
					using (var client = this.webClientBuilder.BuildWebClient())
					{
						var results = client.UploadData(url, method, bytes);
						return Encoding.Default.GetString(results);
					}
				});
		}

		protected Task<string> GetJson(string url)
		{
			url.CheckWhetherArgumentIsNull("url");

			return Task<string>.Factory.StartNew(
				() =>
				{
					using (var client = this.webClientBuilder.BuildWebClient())
					{
						var result = client.DownloadData(new Uri(url));

						return Encoding.Default.GetString(result);
					}
				});
		}

		protected IDictionary<string, string> DeserializeIntoDictionary(string result)
		{
			result.CheckWhetherArgumentIsNull("result");
			return JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
		}
	}
}
