namespace Roylance.WebHCatTests.TestBase
{
	using System;
	using System.Linq;
	using System.Text;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Roylance.WebHCatalogLib;
	using Roylance.WebHCatalogLib.Models;

	public abstract class WebHCatalogClientTestBase
	{
		private const string UserName = "root";

		private const string ServerUrl = "localhost";

		private const int HCatalogPort = 50111;

		private static readonly Random RandomGenerator = new Random();

		protected const string DatabaseName = "integrationtestdatabase";

		protected const string DatabaseLocation = "/databases";

		private static readonly ServerInformation HCatalogInformation = new ServerInformation(ServerUrl, HCatalogPort);

		protected static IWebHCatalogClient BuildHCatalogClient()
		{
			return new WebHCatalogClient(HCatalogInformation, new Credential(UserName));
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
			const string Alphabet = "abcdefghijklmnopqrstuvwxyz";
			var workspace = new StringBuilder();
			
			for (var i = 0; i < 7; i++)
			{
				var idx = RandomGenerator.Next(0, Alphabet.Length - 1);
				workspace.Append(Alphabet[idx]);
			}

			return workspace.ToString();
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
