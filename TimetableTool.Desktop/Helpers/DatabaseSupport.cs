using Caliburn.Micro;
using DataAccess.Library.Logic;
using Logging.Library;
using SQLiteDataAccess.Library;
using System;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace TimetableTool.Desktop.Helpers
	{
	public class DatabaseSupport
		{
		public static void DatabaseUserInitialization()
			{
			// Make sure a database exists.
			int databaseExists = SQLiteData.InitDatabase(Settings.ConnectionString, Settings.DatabasePath);
			Settings.DatabaseVersion = VersionDataAccess.GetCurrentDatabaseVersion();
			DeleteTableMigration(); // Checks if the loaded database version can support the Delete functions
			if (Settings.DatabaseVersion < 3 && databaseExists == 1)
				{
				// Execute a migration to make sure the renaming of the tables Services to ServiceTemplates and ServiceInstances to Services is Executed.
				// One this is done, the database version will be upgraded to 3.
				ServiceTemplateMigration();
				}

			if (Settings.DatabaseVersion >= 3 && Settings.DatabaseVersion < 5)
				{
				// You need ServiceClasses table end the Time Eventstable pus their data initialization.
				SQLiteData.CreateTable("SQL\\UpdateToVersion5.sql");
				Settings.DatabaseVersion = VersionDataAccess.GetCurrentDatabaseVersion();
				}

			if (Settings.UseDemoData && databaseExists == 0)
				{
				DemoDataSetup();
				}
			}

		public static void DemoDataSetup()
			{
			var importHH = new ImportRouteDataAccess("SQL\\HH-testset.ttt");
			var importWSR = new ImportRouteDataAccess("SQL\\WSR-testset.ttt");
			}

		public static void DeleteTableMigration()
			{

			if (Settings.DatabaseVersion < 2)
				{
				Log.Trace("Deprecated database version. Check user manual ch3.2 for a solution.",
					LogEventType.Event);
				}
			}

		public static void ServiceTemplateMigration()
			{
			// Make sure to never do this twice!
			var sql = "SELECT 1 FROM sqlite_master WHERE type='table' AND name='ServiceTemplates'";
			int tableExists = SQLiteData.LoadData<int, dynamic>(sql, new { }, SQLiteData.GetConnectionString()).FirstOrDefault();
			if (tableExists == 0)
				{
				sql = File.ReadAllText("SQL\\ServiceTemplatesChange.sql");
				SQLiteData.SaveData<dynamic>(sql, new { }, SQLiteData.GetConnectionString());

				if (Settings.DatabaseVersion == 2)
					{
					VersionDataAccess.UpdateDatabaseVersion(3);
					}
				}
			}

		}
	}
