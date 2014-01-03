namespace Roylance.WebHCatalogLib.Resources
{
	using Roylance.WebHCatalogLib.Models;

	public class UrlBuilder
	{
		private readonly string userName;

		private readonly bool useKerberos;

		private readonly ServerInformation hCatInformation;

		public UrlBuilder(ServerInformation hCatInformation, Credential credential, bool useKerberos)
		{
			credential.CheckWhetherArgumentIsNull("credential");
			hCatInformation.CheckWhetherArgumentIsNull("hCatInformation");

			this.hCatInformation = hCatInformation;
			this.userName = credential.UserName;
			this.useKerberos = useKerberos;
		}

		public string GetDatabasesUrl()
		{
			const string GetDatabaseTemplate = "http://{0}:{1}/templeton/v1/ddl/database";
			const string GetDatabaseTemplateNoKerberos = GetDatabaseTemplate + "?user.name={2}";

			return this.useKerberos
				       ? GetDatabaseTemplate.FormatTemplate(this.hCatInformation.Server, this.hCatInformation.Port)
							 : GetDatabaseTemplateNoKerberos.FormatTemplate(this.hCatInformation.Server, this.hCatInformation.Port, this.userName);
		}

		public string CreateDatabaseUrl(string database)
		{
			database.CheckWhetherArgumentIsNull("database");

			const string CreateDatabaseTemplate = "http://{0}:{1}/templeton/v1/ddl/database/{2}";
			const string CreateDatabaseTemplateNoKerberos = CreateDatabaseTemplate + "?user.name={3}";

			return this.useKerberos
				       ? CreateDatabaseTemplate.FormatTemplate(this.hCatInformation.Server, this.hCatInformation.Port, database)
				       : CreateDatabaseTemplateNoKerberos.FormatTemplate(this.hCatInformation.Server, this.hCatInformation.Port, database, this.userName);
		}

		public string DeleteDatabaseUrl(string database)
		{
			database.CheckWhetherArgumentIsNull("database");

			const string DeleteDatabaseTemplate = "http://{0}:{1}/templeton/v1/ddl/database/{2}";
			const string DeleteDatabaseTemplateNoKerberos = DeleteDatabaseTemplate + "?user.name={3}";

			return this.useKerberos
				       ? DeleteDatabaseTemplate.FormatTemplate(this.hCatInformation.Server, this.hCatInformation.Port, database)
				       : DeleteDatabaseTemplateNoKerberos.FormatTemplate(
					       this.hCatInformation.Server,
					       this.hCatInformation.Port,
					       database,
					       this.userName);
		}

		public string GetTablesUrl(string database)
		{
			database.CheckWhetherArgumentIsNull("database");

			const string GetTablesTemplate = "http://{0}:{1}/templeton/v1/ddl/database/{2}/table";
			const string GetTablesTemplateNoKerberos = GetTablesTemplate + "?user.name={3}";

			return this.useKerberos
				       ? GetTablesTemplate.FormatTemplate(this.hCatInformation.Server, this.hCatInformation.Port, database)
				       : GetTablesTemplateNoKerberos.FormatTemplate(
					       this.hCatInformation.Server,
					       this.hCatInformation.Port,
					       database,
					       this.userName);
		}

		public string CreateTableUrl(string database, string table)
		{
			database.CheckWhetherArgumentIsNull("database");
			table.CheckWhetherArgumentIsNull("table");

			const string CreateTableTemplate = "http://{0}:{1}/templeton/v1/ddl/database/{2}/table/{3}";
			const string CreateTableTemplateNoKerberos = CreateTableTemplate + "?user.name={4}";

			return this.useKerberos
				       ? CreateTableTemplate.FormatTemplate(this.hCatInformation.Server, this.hCatInformation.Port, database, table)
				       : CreateTableTemplateNoKerberos.FormatTemplate(
					       this.hCatInformation.Server,
					       this.hCatInformation.Port,
					       database,
					       table,
					       this.userName);
		}

		public string DeleteTableUrl(string database, string table)
		{
			database.CheckWhetherArgumentIsNull("database");
			table.CheckWhetherArgumentIsNull("table");

			const string DeleteTableTemplate = "http://{0}:{1}/templeton/v1/ddl/database/{2}/table/{3}";
			const string DeleteTableTemplateNoKerberos = DeleteTableTemplate + "?user.name={4}";

			return this.useKerberos
				       ? DeleteTableTemplate.FormatTemplate(this.hCatInformation.Server, this.hCatInformation.Port, database, table)
				       : DeleteTableTemplateNoKerberos.FormatTemplate(
					       this.hCatInformation.Server,
					       this.hCatInformation.Port,
					       database,
					       table,
					       this.userName);
		}
	}
}
