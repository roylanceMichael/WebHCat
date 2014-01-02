namespace Roylance.WebHCatalogLib.Resources
{
	using System;
	using System.Net;
	using System.Text;

	using Newtonsoft.Json;

	public class RestfulWebService : WebClient, IRestfulWebService
	{
		private const string Post = "POST";

		private const string Put = "PUT";

		private const string Delete = "DELETE";

		protected string SendJson(string url, string data, string method)
		{
			url.CheckWhetherArgumentIsNull("url");
			data.CheckWhetherArgumentIsNull("data");
			method.CheckWhetherArgumentIsNull("method");

			var bytes = Encoding.Default.GetBytes(data);
			try
			{
				var results = this.UploadData(url, method, bytes);
				return Encoding.Default.GetString(results);
			}
			catch (WebException e)
			{
				var errorObj = new { Error = e.Message };
				return JsonConvert.SerializeObject(errorObj, Utilities.DefaultSerializerSettings);
			}
		}

		public T GetJson<T>(string url)
		{
			url.CheckWhetherArgumentIsNull("url");
			var result = this.DownloadData(new Uri(url));
			var strResult = Encoding.Default.GetString(result);
			return JsonConvert.DeserializeObject<T>(strResult, Utilities.DefaultSerializerSettings);
		}

		public T DeleteJson<T>(string url, string data)
		{
			url.CheckWhetherArgumentIsNull("url");
			data.CheckWhetherArgumentIsNull("data");
			var result = this.SendJson(url, data, Delete);
			return JsonConvert.DeserializeObject<T>(result, Utilities.DefaultSerializerSettings);
		}

		public T PostJson<T>(string url, string data)
		{
			url.CheckWhetherArgumentIsNull("url");
			data.CheckWhetherArgumentIsNull("data");
			var result = this.SendJson(url, data, Post);
			return JsonConvert.DeserializeObject<T>(result, Utilities.DefaultSerializerSettings);
		}

		public T PutJson<T>(string url, string data)
		{
			url.CheckWhetherArgumentIsNull("url");
			data.CheckWhetherArgumentIsNull("data");
			var result = this.SendJson(url, data, Put);
			return JsonConvert.DeserializeObject<T>(result, Utilities.DefaultSerializerSettings);
		}
	}
}
