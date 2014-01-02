namespace Roylance.WebHCatalogLib.Models
{
	using Newtonsoft.Json;

	public class Column
	{
		public string Name { get; set; }

		public string Type { get; set; }

		public string Comment { get; set; }

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}
