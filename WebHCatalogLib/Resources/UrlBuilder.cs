namespace Roylance.WebHCatalogLib.Resources
{
	using Roylance.WebHCatalogLib.Models;

	public class UrlBuilder
	{
		private readonly string userName;

		private readonly bool useKerberos;

		private readonly ServerInfoModel hCatInfo;

		public UrlBuilder(ServerInfoModel hCatInfo, CredentialModel credential, bool useKerberos)
		{
			credential.CheckWhetherArgumentIsNull("credential");
			hCatInfo.CheckWhetherArgumentIsNull("hCatInfo");

			this.hCatInfo = hCatInfo;
			this.userName = credential.UserName;
			this.useKerberos = useKerberos;
		}

		public string GetDatabasesUrl()
		{
			const string GetDatabaseTemplate = "http://{0}:{1}/templeton/v1/ddl/database";
			const string GetDatabaseTemplateNoKerberos = GetDatabaseTemplate + "?user.name={2}";

			return this.useKerberos
				       ? GetDatabaseTemplate.FormatTemplate(this.hCatInfo.Server, this.hCatInfo.Port)
							 : GetDatabaseTemplateNoKerberos.FormatTemplate(this.hCatInfo.Server, this.hCatInfo.Port, this.userName);
		}

		public string CreateDatabaseUrl(string database)
		{
			const string CreateDatabaseTemplate = "http://{0}:{1}/templeton/v1/ddl/database/{2}";
			const string CreateDatabaseTemplateNoKerberos = CreateDatabaseTemplate + "?user.name={3}";

			return this.useKerberos
				       ? CreateDatabaseTemplate.FormatTemplate(this.hCatInfo.Server, this.hCatInfo.Port, database)
				       : CreateDatabaseTemplateNoKerberos.FormatTemplate(this.hCatInfo.Server, this.hCatInfo.Port, database, this.userName);
		}

		public string DeleteDatabaseUrl(string database)
		{
			const string DeleteDatabaseTemplate = "http://{0}:{1}/templeton/v1/ddl/database/{2}";
			const string DeleteDatabaseTemplateNoKerberos = DeleteDatabaseTemplate + "?user.name={3}";

			return this.useKerberos
				       ? DeleteDatabaseTemplate.FormatTemplate(this.hCatInfo.Server, this.hCatInfo.Port, database)
				       : DeleteDatabaseTemplateNoKerberos.FormatTemplate(
					       this.hCatInfo.Server,
					       this.hCatInfo.Port,
					       database,
					       this.userName);
		}
	}
}
