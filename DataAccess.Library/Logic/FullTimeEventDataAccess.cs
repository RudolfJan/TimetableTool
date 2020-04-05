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
      string sql = @"INSERT OR IGNORE INTO TimeEvents (EventType, RelativeTime, ServiceId, LocationId, Order)
                      VALUES(@EventType, @RelativeTime, @ServiceId, @LocationId, @Order)";
      return SQLiteData.SaveData<dynamic>(sql, new { timeEvent.EventType, timeEvent.RelativeTime, timeEvent.ServiceId, timeEvent.LocationId, timeEvent.Order }, SQLiteData.GetConnectionString());
      }
    }
  }
