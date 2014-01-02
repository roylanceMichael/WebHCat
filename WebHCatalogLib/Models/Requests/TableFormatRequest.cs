namespace Roylance.WebHCatalogLib.Models
{
	public class TableFormatRequest
	{
		public string StoredAs { get; set; }

		public RowFormatRequest RowFormat { get; set; }
	}
}
