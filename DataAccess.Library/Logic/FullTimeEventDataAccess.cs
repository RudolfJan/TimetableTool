using DataAccess.Library.Models;
using SQLiteDataAccess.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Library.Logic
  {
  public class FullTimeEventDataAccess
    {
    public static List<FullTimeEventModel> GetAllFullTimeEventsPerService(int serviceId)
      {
      string sql = "SELECT * FROM FullTimeEvents WHERE ServiceId=@ServiceId";

      var timeEventList =
        SQLiteData.LoadData<FullTimeEventModel, dynamic>(sql, new { serviceId}, SQLiteData.GetConnectionString()).ToList();
      return timeEventList;
      }

    public static List<FullTimeEventModel> CreateTimeEventsPerService(int serviceId)
      {
      string sql="SELECT ServiceDirections.IsDescending FROM ServiceDirections, Services WHERE Services.Id=@ServiceId AND ServiceDirections.Id=Services.ServiceDirectionId;";
      var isDescending= SQLiteData.LoadData<int,dynamic>(sql, new{serviceId},SQLiteData.GetConnectionString()).FirstOrDefault();
      string direction;

      if(isDescending==1)
        {
        direction="DESC";
        }
      else
        {
        direction="ASC";
        }

      sql = $"SELECT Locations.Id, Locations.LocationName, Locations.LocationAbbreviation, Locations.NumberOfTracks, Locations.[Order], Locations.Routeid FROM Locations, Services WHERE Services.Id=@ServiceId AND Locations.RouteId= Services.RouteId ORDER BY Locations.[Order] {direction};";

      var locations =
        SQLiteData.LoadData<LocationModel, dynamic>(sql, new { serviceId}, SQLiteData.GetConnectionString()).ToList();

      var timeEventList= new List<FullTimeEventModel>();
      int order=10;
      // Warnig: No all columns are filled!
      // TODO refactor this, using extraneous fields.
      foreach(var location in locations)
        {
        var timeEvent= new FullTimeEventModel();
        timeEvent.Order=order;
        order+=10;
        timeEvent.LocationId= location.Id;
        timeEvent.ServiceId= serviceId;
        timeEvent.LocationName= location.LocationName;
        timeEventList.Add(timeEvent);
        }
      return timeEventList;
      }


    public static FullTimeEventModel GetFullTimeEventById(int timeEventId)
      {
      string sql = "SELECT * FROM FullTimeEvents WHERE Id= @TimeEventId";

      var timeEvent =
        SQLiteData.LoadData<FullTimeEventModel, dynamic>(sql, new {timeEventId}, SQLiteData.GetConnectionString()).FirstOrDefault();
      return timeEvent;
      }

    public static int InsertFullTimeEventForService(FullTimeEventModel timeEvent)
      {
      // TODO consider re-use of the timeEven 
      string sql = @"INSERT OR IGNORE INTO TimeEvents (EventType, ArrivalTime, WaitTime, ServiceId, LocationId, Order)
                      VALUES(@EventType, @RelativeTime, @ServiceId, @LocationId, @Order)";
      return SQLiteData.SaveData<dynamic>(sql, new { timeEvent.EventType, timeEvent.ArrivalTime, timeEvent.WaitTime, timeEvent.ServiceId, timeEvent.LocationId, timeEvent.Order }, SQLiteData.GetConnectionString());
      }
    }
  }
