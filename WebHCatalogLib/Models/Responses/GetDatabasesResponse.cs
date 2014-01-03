namespace Roylance.WebHCatalogLib.Models
{
	using System.Collections.Generic;

	public class GetDatabasesResponse : ResponseBase
	{
		public IEnumerable<string> Databases { get; set; } 
	}
}
