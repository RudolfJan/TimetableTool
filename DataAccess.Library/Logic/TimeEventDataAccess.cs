using DataAccess.Library.Models;
using SQLiteDataAccess.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Library.Logic
  {
  public class TimeEventDataAccess
    {
    public static List<TimeEventModel> GetAllTimeEventsPerService(int serviceId)
      {
      string sql = "SELECT * FROM TimeEvents WHERE ServiceId=@ServiceId";

      var timeEventList =
        SQLiteData.LoadData<TimeEventModel, dynamic>(sql, new { serviceId}, SQLiteData.GetConnectionString()).ToList();
      return timeEventList;
      }

 
    public static TimeEventModel GetTimeEventById(int timeEventId)
      {
      string sql = "SELECT * FROM TimeEvents WHERE Id= @TimeEventId";

      var timeEvent =
        SQLiteData.LoadData<TimeEventModel, dynamic>(sql, new {timeEventId}, SQLiteData.GetConnectionString()).FirstOrDefault();
      return timeEvent;
      }

    public static int InsertTimeEventForService(TimeEventModel timeEvent)
      {
      string sql = @"INSERT OR IGNORE INTO TimeEvents (EventType, ArrivalTime, WaitTime, ServiceId, LocationId, [Order])
                      VALUES(@EventType, @ArrivalTime, @WaitTime, @ServiceId, @LocationId, @Order)";
      return SQLiteData.SaveData<dynamic>(sql,new {timeEvent.EventType, timeEvent.ArrivalTime, timeEvent.WaitTime, timeEvent.ServiceId, timeEvent.LocationId, timeEvent.Order}, SQLiteData.GetConnectionString());
      }

    public static void UpdateTimeEvent(TimeEventModel timeEvent)
      {
      string sql = @"UPDATE OR IGNORE TimeEvents SET EventType=@EventType, ArrivalTime=@ArrivalTime, WaitTime=@WaitTime, ServiceId=@ServiceId, LocationId=@LocationId, [Order]=@Order WHERE Id=@Id";
      SQLiteData.SaveData<dynamic>(sql,new {timeEvent.EventType, timeEvent.ArrivalTime,
        timeEvent.WaitTime, timeEvent.ServiceId, timeEvent.LocationId, timeEvent.Order, timeEvent.Id}, SQLiteData.GetConnectionString());
      }
    }
  }
