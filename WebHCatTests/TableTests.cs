namespace Roylance.WebHCatTests
{
	using System.Collections.Generic;
	using System.Linq;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Roylance.WebHCatalogLib.Constants;
	using Roylance.WebHCatalogLib.Models;
	using Roylance.WebHCatTests.TestBase;

	[TestClass]
	public class TableTests : WebHCatalogClientTestBase
	{
		[TestMethod]
		public void CreatesTableWithOneColumn()
		{
			// arrange
			this.CreateDatabaseIfNotExists();
			var uniqueTableName = BuildUniqueName();

			var createTableRequest = new CreateTableRequest
																 {
																	 Database = DatabaseName,
																	 Table = uniqueTableName,
																	 Comment = "what is this",
																	 Location = "/testtable",
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

		[TestMethod]
		public void CreatesTableWithSixColumns()
		{
			// arrange
			this.CreateDatabaseIfNotExists();
			var uniqueTableName = BuildUniqueName();

			var createTableRequest = new CreateTableRequest
			{
				Database = DatabaseName,
				Table = uniqueTableName,
				Comment = "what is this",
				Location = "/testtable",
				Format = new TableFormatRequest
									 {
										 RowFormat = new RowFormatRequest
																	 {
																		 FieldsTerminatedBy = ","
																	 },
										 StoredAs = TableFormats.TextFile
									 },
				Columns = new List<Column>
																		           {
																			           new Column
																				           {
																					           Name = "a",
																										 Type = ColumnTypes.String
																				           },
																									new Column
																				           {
																					           Name = "b",
																										 Type = ColumnTypes.String
																				           },
																									 new Column
																				           {
																					           Name = "c",
																										 Type = ColumnTypes.String
																				           },
																									 new Column
																				           {
																					           Name = "d",
																										 Type = ColumnTypes.String
																				           },
																									 new Column
																				           {
																					           Name = "e",
																										 Type = ColumnTypes.String
																				           },
																									 new Column
																				           {
																					           Name = "f",
																										 Type = ColumnTypes.String
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

		[TestMethod]
		public void DeletesTable()
		{
			// arrange
			this.CreateDatabaseIfNotExists();
			var uniqueTableName = BuildUniqueName();

			var createTableRequest = new CreateTableRequest
			{
				Database = DatabaseName,
				Table = uniqueTableName,
				Comment = "what is this",
				Location = "/testtable",
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
			var task = client.CreateTable(createTableRequest);
			task.Wait();
			Assert.IsTrue(task.Result);

			var getTablesTask = client.GetTables(DatabaseName);
			getTablesTask.Wait();

			Assert.IsTrue(getTablesTask.Result.Tables.Contains(uniqueTableName));

			// act
			var deleteTableTask = client.DeleteTable(DatabaseName, uniqueTableName);
			deleteTableTask.Wait();

			// assert
			Assert.IsTrue(deleteTableTask.Result);
			getTablesTask = client.GetTables(DatabaseName);
			getTablesTask.Wait();
			Assert.IsFalse(getTablesTask.Result.Tables.Contains(uniqueTableName));
		}
	}
}
