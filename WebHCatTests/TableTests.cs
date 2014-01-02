namespace Roylance.WebHCatTests
{
	using System.Collections.Generic;
	using System.Linq;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Roylance.WebHCatalogLib.Models;
	using Roylance.WebHCatTests.TestBase;

	[TestClass]
	public class TableTests : WebHCatalogClientTestBase
	{
		[TestMethod]
		public void CreatesTableWithNoColumns()
		{
			// arrange
			this.CreateDatabaseIfNotExists();
			var uniqueTableName = BuildUniqueName();

			var createTableRequest = new CreateTableRequest
				                         {
					                         Database = DatabaseName, 
																	 Table = uniqueTableName, 
																	 Comment = "what is this",
																	 Columns = new List<Column>
																		           {
																			           new Column
																				           {
																					           Name = "id",
																										 Type = ColumnTypes.BigInt
																				           }
																		           }

				                         };

			var client = BuildHCatalogClient();

			// act
			var task = client.CreateTable(createTableRequest);
			task.Wait();

			// assert
			Assert.IsTrue(task.Result);

			var getTablesTask = client.GetTables(DatabaseName);
			getTablesTask.Wait();

			Assert.IsTrue(getTablesTask.Result.Tables.Contains(uniqueTableName));
		}
	}
}
