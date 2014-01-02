namespace Roylance.WebHCatalogLib.Models
{
	using System.Collections.Generic;

	using Newtonsoft.Json;

	using Roylance.WebHCatalogLib.Resources;

	public class CreateTableRequest : RequestBase
	{
		[JsonIgnore]
		public string Database { get; set; }

		[JsonIgnore]
		public string Table { get; set; }

		public string Comment { get; set; }

		public string Location { get; set; }

		public TableFormatRequest Format { get; set; }

		public string External
		{
			get
			{
				return bool.TrueString.ToLower();
			}
		}

		public IEnumerable<Column> Columns { get; set; }

		public override string ToJson(params object[] args)
		{
			return JsonConvert.SerializeObject(this, Utilities.DefaultSerializerSettings);
		}
	}
}
