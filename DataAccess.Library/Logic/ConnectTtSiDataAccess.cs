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
			string sql = "SELECT ConnectTtSi.Id, ConnectTtSi.TimetableId, ConnectTtSi.ServiceId FROM ConnectTtSi, Timetables WHERE ConnectTtSi.TimetableId= Timetables.Id AND Timetables.RouteId=@RouteId";

			var connectList =
				SQLiteData.LoadData<ConnectTtSiModel, dynamic>(sql, new {routeId }, SQLiteData.GetConnectionString()).ToList();
			return connectList;
			}

		  public static void InsertConnection(int serviceId, int timetableId)
      {
			string sql = @"INSERT OR IGNORE INTO ConnectTtSi (ServiceId, TimetableId) 
                                    VALUES(@ServiceId, @TimetableId);";
      SQLiteData.SaveData<dynamic>(sql,
        new {serviceId, timetableId}, SQLiteData.GetConnectionString());
      }

    public static void DeleteConnection(int serviceId, int timetableId)
      {
			string sql = "DELETE FROM ConnectTtSi WHERE ServiceId=@ServiceId AND TimetableId=@TimetableId";
      SQLiteData.SaveData<dynamic>(sql,
        new {serviceId, timetableId}, 
        SQLiteData.GetConnectionString());
      }

		}
	}
