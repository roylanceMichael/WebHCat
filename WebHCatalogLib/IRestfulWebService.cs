namespace Roylance.WebHCatalogLib
{
	using System;

	public interface IRestfulWebService: IDisposable
	{
		T DeleteJson<T>(string url, string data);
		
		T GetJson<T>(string url);

		T PostJson<T>(string url, string data);

		T PutJson<T>(string url, string data);
	}
}
