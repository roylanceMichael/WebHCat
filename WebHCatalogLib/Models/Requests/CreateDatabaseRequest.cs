namespace Roylance.WebHCatalogLib.Models
{
	using Newtonsoft.Json;

	using Roylance.WebHCatalogLib.Resources;

	public class CreateDatabaseRequest : RequestBase
	{
		[JsonIgnore]
		public string Database { get; set; }

		public string Comment { get; set; }

		public string Location { get; set; }

		public override string ToJson(params object[] args)
		{
			return JsonConvert.SerializeObject(this, Utilities.DefaultSerializerSettings);
		}
	}
}
