using DataAccess.Library.Models;
using SQLiteDataAccess.Library;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Library.Logic
	{
	public class RouteDataAccess
		{

		public static List<RouteModel> GetAllRoutes()
			{
			string sql = "SELECT * FROM Routes";

			var routesList =
				SQLiteData.LoadData<RouteModel, dynamic>(sql, new { }, SQLiteData.GetConnectionString()).ToList();
			return routesList;
			}

		public static RouteModel GetRouteById(int routeId)
			{
			string sql = "SELECT * FROM Routes WHERE Id= @RouteId";

			var route =
				SQLiteData.LoadData<RouteModel, dynamic>(sql, new { routeId }, SQLiteData.GetConnectionString()).FirstOrDefault();
			return route;
			}

		public static int InsertRoute(RouteModel route)
			{
			string sql = "INSERT OR IGNORE INTO Routes (RouteName, RouteAbbreviation, RouteDescription) VALUES(@RouteName, @RouteAbbreviation, @RouteDescription);SELECT last_insert_rowid();";
			return SQLiteData.SaveData<dynamic>(sql, new { route.RouteName, route.RouteAbbreviation, route.RouteDescription }, SQLiteData.GetConnectionString());
			}

		public static void UpdateRoute(RouteModel route)
			{
			string sql = "UPDATE OR IGNORE Routes SET RouteName=@RouteName, RouteAbbreviation=@RouteAbbreviation, RouteDescription=@RouteDescription WHERE Id= @Id";
			SQLiteData.SaveData<dynamic>(sql, new { route.RouteName, route.RouteAbbreviation, route.RouteDescription, route.Id }, SQLiteData.GetConnectionString());
			}

		public static void DeleteRoute(int routeId)
			{
			string sql = "PRAGMA foreign_keys = ON;DELETE FROM Routes WHERE Routes.Id=@RouteId;";
			SQLiteData.SaveData<dynamic>(sql, new { routeId }, SQLiteData.GetConnectionString());
			}
		}
	}
