namespace Roylance.WebHCatalogLib.Models
{
	public abstract class RequestBase
	{
		public abstract string ToJson(params object[] args);
	}
}
