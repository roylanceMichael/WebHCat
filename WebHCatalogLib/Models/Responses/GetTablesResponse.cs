using System.Collections.Generic;

namespace Roylance.WebHCatalogLib.Models
{
	public class GetTablesResponse
	{
		public string Database { get; set; }

		public IEnumerable<string> Tables { get; set; } 
	}
}
