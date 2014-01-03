namespace Roylance.WebHCatTests
{
	using System.Linq;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Roylance.WebHCatalogLib.Models;
	using Roylance.WebHCatTests.TestBase;

	[TestClass]
	public class DatabaseTests : WebHCatalogClientTestBase
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
			var uniqueName = BuildUniqueName();

			VerifyDatabase(uniqueName, false);

			var client = BuildHCatalogClient();

			var createDatabaseRequest = new CreateDatabaseRequest { Database = uniqueName, Location = DatabaseLocation };

			var createDbTask = client.CreateDatabase(createDatabaseRequest);
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
