using SQLiteDataAccess.Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Library.Logic
	{
	public class ConnectTtSiDataAccess
		{
		  public static void InsertConnection(int serviceInstanceId, int timetableId)
      {
      string sql = @"INSERT OR IGNORE INTO ConnectTtSi (ServiceInstanceId, TimetableId) 
                                    VALUES(@ServiceInstanceId, @TimetableId);";
      SQLiteData.SaveData<dynamic>(sql,
        new {serviceInstanceId, timetableId}, SQLiteData.GetConnectionString());
      }

    public static void DeleteConnection(int serviceInstanceId, int timetableId)
      {
      string sql = "DELETE OR IGNORE FROM ConnectTtSi WHERE ServiceInstanceId=@ServiceInstanceId AND TimetableId=@TimetableId";
      SQLiteData.SaveData<dynamic>(sql,
        new {serviceInstanceId, timetableId}, 
        SQLiteData.GetConnectionString());
      }

		}
	}
