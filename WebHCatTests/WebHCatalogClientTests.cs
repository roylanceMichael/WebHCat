namespace Roylance.WebHCatTests
{
	using System;
	using System.Linq;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Roylance.WebHCatalogLib;
	using Roylance.WebHCatalogLib.Models;

	public class WebHCatalogClientTests
	{
		private const string UserName = "root";

		private const string ServerUrl = "127.0.0.1";

		private const int HCatalogPort = 50111;

		private const string HdfsUrl = "10.0.2.15";

		private const int HdfsPort = 8020;

		private static readonly ServerInfoModel HCatalogInfo = new ServerInfoModel(ServerUrl, HCatalogPort);

		private static readonly ServerInfoModel HdfsInfo = new ServerInfoModel(HdfsUrl, HdfsPort);

		private static IWebHCatalogClient BuildHCatalogClient()
		{
			return new WebHCatalogClient(HCatalogInfo, HdfsInfo, new CredentialModel(UserName));
		}

		private static string BuildUniqueName()
		{
			return Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 10);
		}

		private static void VerifyDatabase(string databaseName, bool verifyExists)
		{
			var client = BuildHCatalogClient();
			var getDbsTask = client.GetDatabases();
			getDbsTask.Wait();
			Assert.AreEqual(verifyExists, getDbsTask.Result.Databases.Contains(databaseName));
		}

		[TestClass]
		public class Databases : WebHCatalogClientTests
		{
			[TestMethod]
			public void DatabasesReturnedWithDefaultUser()
			{
				// arrange
				var client = BuildHCatalogClient();

				// act
				var task = client.GetDatabases();
				task.Wait();

				// assert
				Assert.IsTrue(task.Result.Databases.Any());
			}

			[TestMethod]
			public void DatabaseDeletedWhenCalledFromClient()
			{
				// arrange
				const string Location = "databases";
				var uniqueName = BuildUniqueName();

				VerifyDatabase(uniqueName, false);

				var client = BuildHCatalogClient();

				var createDbTask = client.CreateDatabase(uniqueName, null, Location);
				createDbTask.Wait();
				Assert.IsTrue(createDbTask.Result);

				var getDbsTask = client.GetDatabases();
				getDbsTask.Wait();

				VerifyDatabase(uniqueName, true);

				// act
				var resultTask = client.DeleteDatabase(uniqueName);
				resultTask.Wait();

				// assert
				Assert.IsTrue(resultTask.Result);
				VerifyDatabase(uniqueName, false);
			}
		}
	}
}
