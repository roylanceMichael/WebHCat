namespace Roylance.WebHCatalogLib.Resources
{
	using System;
	using System.Net;
	using System.Text;

	using Newtonsoft.Json;
	using Newtonsoft.Json.Serialization;

	public class RestfulWebService : WebClient, IRestfulWebService
	{
		private const string Post = "POST";

		private const string Put = "PUT";

		private const string Delete = "DELETE";

		private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

		protected string SendJson(string url, string data, string method)
		{
			url.CheckWhetherArgumentIsNull("url");
			data.CheckWhetherArgumentIsNull("data");
			method.CheckWhetherArgumentIsNull("method");

			var bytes = Encoding.Default.GetBytes(data);
			var results = this.UploadData(url, method, bytes);
			return Encoding.Default.GetString(results);
		}

		public T GetJson<T>(string url)
		{
			url.CheckWhetherArgumentIsNull("url");
			var result = this.DownloadData(new Uri(url));
			var strResult = Encoding.Default.GetString(result);
			return JsonConvert.DeserializeObject<T>(strResult, this.serializerSettings);
		}

		public T DeleteJson<T>(string url, string data)
		{
			url.CheckWhetherArgumentIsNull("url");
			data.CheckWhetherArgumentIsNull("data");
			var result = this.SendJson(url, data, Delete);
			return JsonConvert.DeserializeObject<T>(result, this.serializerSettings);
		}

		public T PostJson<T>(string url, string data)
		{
			url.CheckWhetherArgumentIsNull("url");
			data.CheckWhetherArgumentIsNull("data");
			var result = this.SendJson(url, data, Post);
			return JsonConvert.DeserializeObject<T>(result, this.serializerSettings);
		}

		public T PutJson<T>(string url, string data)
		{
			url.CheckWhetherArgumentIsNull("url");
			data.CheckWhetherArgumentIsNull("data");
			var result = this.SendJson(url, data, Put);
			return JsonConvert.DeserializeObject<T>(result, this.serializerSettings);
		}
	}
}
