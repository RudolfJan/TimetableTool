using DataAccess.Library.Models;
using SQLiteDataAccess.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Library.Logic
  {
  public class TimetableDataAccess
    {
    public static List<TimetableModel> GetAllTimetablesPerRoute(int routeId)
      {
      string sql = "SELECT * FROM Timetables WHERE RouteId=@RouteId";

      var timetableList =
        SQLiteData.LoadData<TimetableModel, dynamic>(sql, new { routeId}, SQLiteData.GetConnectionString()).ToList();
      return timetableList;
      }

    public static TimetableModel GetTimetableById(int timeTableId)
      {
      string sql = "SELECT * FROM Timetables WHERE Id= @timeTableId";

      var timeTable =
        SQLiteData.LoadData<TimetableModel, dynamic>(sql, new {timeTableId}, SQLiteData.GetConnectionString()).FirstOrDefault();
      return timeTable;
      }

    public static int InsertTimetableForRoute(TimetableModel timetable)
      {
      string sql = @"INSERT OR IGNORE INTO Timetables 
                   (TimetableName, TimetableAbbreviation, TimetableDescription, 
                    ServiceDirectionId, IsMultiDirection, RouteId) 
                    VALUES(@TimetableName, @TimetableAbbreviation, @TimetableDescription, 
                    @ServiceDirectionId, @IsMultiDirection, @RouteId)";
      return SQLiteData.SaveData<dynamic>(sql,new {timetable.TimetableName, timetable.TimetableAbbreviation, 
                                          timetable.TimetableDescription, timetable.ServiceDirectionId, 
                                          timetable.IsMultiDirection, timetable.RouteId}, 
                                          SQLiteData.GetConnectionString());
      }

    public static void UpdateTimetable(TimetableModel timetable)
      {
      string sql = @"UPDATE OR IGNORE Timetables 
                          SET 
                                  TimetableName=@TimetableName, 
                                  TimetableAbbreviation=@TimetableAbbreviation, 
                                  TimetableDescription=@TimetableDescription, 
                                  ServiceDirectionId=@ServiceDirectionId,
                                  IsMultiDirection=@IsMultiDirection,
                                  RouteId=@RouteId WHERE Id=@Id";
      SQLiteData.SaveData<dynamic>(sql,new {timetable.TimetableName, timetable.TimetableAbbreviation, 
                                            timetable.TimetableDescription, timetable.ServiceDirectionId,
                                            timetable.IsMultiDirection, timetable.RouteId, timetable.Id}, 
                                            SQLiteData.GetConnectionString());
      }
    }
  }
