using DataAccess.Library.Models;
using SQLiteDataAccess.Library;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Library.Logic
  {
  public class TimeEventDataAccess
    {
    public static List<TimeEventModel> GetAllTimeEventsPerServiceTemplate(int serviceTemplateId)
      {
      string sql = "SELECT * FROM TimeEvents " +
                   "WHERE ServiceTemplateId=@ServiceTemplateId";

      var timeEventList =
        SQLiteData.LoadData<TimeEventModel, dynamic>(sql, new { serviceTemplateId}, SQLiteData.GetConnectionString()).ToList();
      return timeEventList;
      }

    public static List<TimeEventModel> GetAllTimeEventsPerRoute(int routeId)
      {
      string sql = "SELECT * FROM TimeEvents, ServiceTemplates " +
                   "WHERE TimeEvents.ServiceTemplateId=ServiceTemplates.Id AND ServiceTemplates.RouteId=@RouteId";

      var timeEventList = SQLiteData.LoadData<TimeEventModel, dynamic>(sql, new { routeId} 
                          ,SQLiteData.GetConnectionString()).ToList();
      return timeEventList;
      }
 
    public static TimeEventModel GetTimeEventById(int timeEventId)
      {
      string sql = "SELECT * FROM TimeEvents " +
                   "WHERE Id= @TimeEventId";

      var timeEvent =
        SQLiteData.LoadData<TimeEventModel, dynamic>(sql, new {timeEventId}, SQLiteData.GetConnectionString()).FirstOrDefault();
      return timeEvent;
      }

    public static int InsertTimeEventForServiceTemplate(TimeEventModel timeEvent)
      {
      string sql = @"INSERT OR IGNORE INTO TimeEvents (EventType, ArrivalTime, WaitTime, ServiceTemplateId, LocationId, [Order])
                      VALUES(@EventType, @ArrivalTime, @WaitTime, @ServiceTemplateId, @LocationId, @Order)";
      return SQLiteData.SaveData<dynamic>(sql,new {timeEvent.EventType, timeEvent.ArrivalTime
                                          ,timeEvent.WaitTime, timeEvent.ServiceTemplateId, timeEvent.LocationId, timeEvent.Order} 
                                          ,SQLiteData.GetConnectionString());
      }

    public static void UpdateTimeEvent(TimeEventModel timeEvent)
      {
      string sql = @"UPDATE OR IGNORE TimeEvents SET EventType=@EventType, ArrivalTime=@ArrivalTime, WaitTime=@WaitTime, ServiceTemplateId=@ServiceTemplateId, LocationId=@LocationId, [Order]=@Order WHERE Id=@Id";
      SQLiteData.SaveData<dynamic>(sql,new {timeEvent.EventType, timeEvent.ArrivalTime,
        timeEvent.WaitTime, timeEvent.ServiceTemplateId, timeEvent.LocationId, timeEvent.Order, timeEvent.Id}, SQLiteData.GetConnectionString());
      }

		public static void DeleteTimeEvent(int timeEventId)
			{
      string sql = "PRAGMA foreign_keys = ON;" +
                   "DELETE FROM TimeEvents WHERE TimeEvents.Id=@TimeEventId;";
      SQLiteData.SaveData<dynamic>(sql, new { timeEventId }, SQLiteData.GetConnectionString());
			}
		}
  }
