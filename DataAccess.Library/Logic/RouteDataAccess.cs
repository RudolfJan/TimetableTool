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


		public static int GetServiceTemplatesCountById(int routeId)
			{
			string sql =
				"SELECT COUNT(*) AS RouteTemplatesCount FROM ServiceTemplates WHERE ServiceTemplates.RouteId= @RouteId;";

			var routeTemplateCount =
				SQLiteData.LoadData<int, dynamic>(sql, new { routeId }, SQLiteData.GetConnectionString()).FirstOrDefault();
			return routeTemplateCount;
			}

		public static int GetTrainCountById(int routeId)
			{
			string sql = "SELECT COUNT(*) AS TrainsCount FROM Trains WHERE Trains.RouteId= @RouteId;";

			var trainCount =
				SQLiteData.LoadData<int, dynamic>(sql, new { routeId }, SQLiteData.GetConnectionString()).FirstOrDefault();
			return trainCount;
			}

		public static int GetLocationCountById(int routeId)
			{
			string sql = "SELECT COUNT(*) AS LocationsCount FROM Locations WHERE Locations.RouteId= @RouteId;";

			var locationCount =
				SQLiteData.LoadData<int, dynamic>(sql, new { routeId }, SQLiteData.GetConnectionString()).FirstOrDefault();
			return locationCount;
			}

		public static List<RouteStatisticsModel> GetRouteStatistics()
			{
			string sql = @"SELECT Routes.Id, Routes.RouteName, Routes.RouteAbbreviation, COUNT(*) AS ServiceCount
												FROM Routes, Services, ServiceTemplates 
												WHERE Services.ServiceTemplateId= ServiceTemplates.Id 
															AND ServiceTemplates.RouteId= Routes.Id
												GROUP BY Routes.RouteAbbreviation;";

			var routeStatisticsList =
				SQLiteData.LoadData<RouteStatisticsModel, dynamic>(sql, new { }, SQLiteData.GetConnectionString()).ToList();
			foreach (var item in routeStatisticsList)
				{
				item.ServiceTemplateCount = GetServiceTemplatesCountById(item.Id);
				item.TrainCount = GetTrainCountById(item.Id);
				item.LocationCount = GetLocationCountById(item.Id);
				}
			return routeStatisticsList;
			}




		}
	}
