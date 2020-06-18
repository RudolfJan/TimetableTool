using SQLiteDataAccess.Library;
using System.Data.SQLite;
using System.Linq;

namespace DataAccess.Library.Logic
	{
	public class VersionDataAccess
		{
		public static int GetCurrentDatabaseVersion()
			{
			string sql1 ="SELECT 1 FROM sqlite_master WHERE type='table' AND name='Version'";

			string sql2 = "SELECT VersionNr FROM Version";
			try
				{
				int tableExists = SQLiteData.LoadData<int, dynamic>(sql1, new {}, SQLiteData.GetConnectionString()).FirstOrDefault();
				if (tableExists == 1)
					{
					return SQLiteData.LoadData<int, dynamic>(sql2, new { }, SQLiteData.GetConnectionString())
						.FirstOrDefault();
					}
				return 0; 
				}
			catch (SQLiteException e)
				{
				return 0;
				}
			}

		public static void UpdateDatabaseVersion(int version)
			{
			string sql = "UPDATE Version SET VersionNr=@version";
			SQLiteData.SaveData<dynamic>(sql, new {version}, SQLiteData.GetConnectionString());
			}
		}
	}
