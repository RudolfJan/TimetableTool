using DataAccess.Library.Models;
using SQLiteDataAccess.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Library.Logic
	{
	public class ConnectTtSiDataAccess
		{

		public static List<ConnectTtSiModel> GetAllConnectTtSiPerRoute(int routeId)
			{
			string sql = "SELECT ConnectTtSi.Id, ConnectTtSi.TimetableId, ConnectTtSi.ServiceInstanceId FROM ConnectTtSi, Timetables WHERE ConnectTtSi.TimetableId= Timetables.Id AND Timetables.RouteId=@RouteId";

			var connectList =
				SQLiteData.LoadData<ConnectTtSiModel, dynamic>(sql, new {routeId }, SQLiteData.GetConnectionString()).ToList();
			return connectList;
			}

		  public static void InsertConnection(int serviceInstanceId, int timetableId)
      {
      string sql = @"INSERT OR IGNORE INTO ConnectTtSi (ServiceInstanceId, TimetableId) 
                                    VALUES(@ServiceInstanceId, @TimetableId);";
      SQLiteData.SaveData<dynamic>(sql,
        new {serviceInstanceId, timetableId}, SQLiteData.GetConnectionString());
      }

    public static void DeleteConnection(int serviceInstanceId, int timetableId)
      {
      string sql = "DELETE FROM ConnectTtSi WHERE ServiceInstanceId=@ServiceInstanceId AND TimetableId=@TimetableId";
      SQLiteData.SaveData<dynamic>(sql,
        new {serviceInstanceId, timetableId}, 
        SQLiteData.GetConnectionString());
      }

		}
	}
