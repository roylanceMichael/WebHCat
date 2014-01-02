namespace Roylance.WebHCatTests.TestBase
{
	using System;
	using System.Linq;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Roylance.WebHCatalogLib;
	using Roylance.WebHCatalogLib.Models;

	public abstract class WebHCatalogClientTestBase
	{
		private const string UserName = "root";

		private const string ServerUrl = "127.0.0.1";

		private const int HCatalogPort = 50111;

		private const string HdfsUrl = "10.0.2.15";

		private const int HdfsPort = 8020;

		protected const string DatabaseName = "integrationtestdatabase";

		protected const string DatabaseLocation = "databases";

		private static readonly ServerInformation HCatalogInformation = new ServerInformation(ServerUrl, HCatalogPort);

		private static readonly ServerInformation HdfsInformation = new ServerInformation(HdfsUrl, HdfsPort);

		protected static IWebHCatalogClient BuildHCatalogClient()
		{
			return new WebHCatalogClient(HCatalogInformation, HdfsInformation, new Credential(UserName));
		}

		protected void CreateDatabaseIfNotExists()
		{
			var client = BuildHCatalogClient();
			var task = client.GetDatabases();
			task.Wait();

			if (task.Result.Databases.Any(s => DatabaseName == s))
			{
				return;
			}

			var createDatabase = new CreateDatabaseRequest { Database = DatabaseName, Location = DatabaseLocation };

			client.CreateDatabase(createDatabase).Wait();
		}

		protected static string BuildUniqueName()
		{
			return Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 10);
		}

		protected static void VerifyDatabase(string databaseName, bool verifyExists)
		{
			var client = BuildHCatalogClient();
			var getDbsTask = client.GetDatabases();
			getDbsTask.Wait();
			Assert.AreEqual(verifyExists, getDbsTask.Result.Databases.Contains(databaseName));
		}
	}
}
