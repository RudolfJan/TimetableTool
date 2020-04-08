using DataAccess.Library.Models;
using SQLiteDataAccess.Library;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Library.Logic
  {
  public class LocationDataAccess
    {
    public static List<LocationModel> GetAllLocationsPerRoute(int routeId)
      {
      string sql = "SELECT * FROM Locations WHERE RouteId=@RouteId";

      var locationList =
        SQLiteData.LoadData<LocationModel, dynamic>(sql, new { routeId}, SQLiteData.GetConnectionString()).ToList();
      return locationList;
      }

    public static LocationModel GetLocationById(int locationId)
      {
      string sql = "SELECT * FROM Locations WHERE Id= @LocationId";

      var location =
        SQLiteData.LoadData<LocationModel, dynamic>(sql, new {locationId}, SQLiteData.GetConnectionString()).FirstOrDefault();
      return location;
      }

    public static int InsertLocationForRoute(LocationModel location)
      {
      string sql = "INSERT OR IGNORE INTO Locations (LocationName, LocationAbbreviation, NumberOfTracks, [Order], RouteId) VALUES(@LocationName, @LocationAbbreviation, @NumberOfTracks, @Order, @RouteId)";
      return SQLiteData.SaveData<dynamic>(sql,new {location.LocationName, location.LocationAbbreviation, location.NumberOfTracks, location.Order, location.RouteId}, SQLiteData.GetConnectionString());
      }

    public static void UpdateLocationForRoute(LocationModel location)
      {
      string sql = "UPDATE OR IGNORE Locations SET LocationName=@LocationName, LocationAbbreviation=@LocationAbbreviation, NumberOfTracks=@NumberOfTracks, [Order]=@Order, RouteId=@RouteId WHERE Id=@Id";
      SQLiteData.SaveData<dynamic>(sql,new {location.LocationName, location.LocationAbbreviation, location.NumberOfTracks, location.Order, location.RouteId, location.Id}, SQLiteData.GetConnectionString());

      }
    }
  }
