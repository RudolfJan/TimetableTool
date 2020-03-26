using DataAccess.Library.Models;
using SQLiteDataAccess.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        SQLiteData.LoadData<RouteModel, dynamic>(sql, new {routeId}, SQLiteData.GetConnectionString()).FirstOrDefault();
      return route;
      }

    public static void InsertRoute(RouteModel route)
      {
      string sql = "INSERT OR IGNORE RouteName, RouteAbbreviation, RouteDescription VALUES(@RouteName, @RouteAbbreviation, @RouteDescription)";
      SQLiteData.SaveData<dynamic>(sql,new {route.RouteName, route.RouteAbbreviation, route.RouteDescription}, SQLiteData.GetConnectionString());
      }
    }
  }
