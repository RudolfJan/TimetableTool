﻿using DataAccess.Library.Models;
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
    public static List<TimetableRouteModel> GetAllTimetables()
      {
      string sql = "SELECT Timetables.Id AS TimetableId, Routes.Id AS RouteId, Timetables.TimetableAbbreviation, Timetables.TimetableName, " +
                   "Timetables.TimetableDescription, Timetables.IsMultiDirection, Timetables.ServiceDirectionId, " +
                   "Routes.RouteName, Routes.RouteAbbreviation " +
                   "FROM Timetables, Routes WHERE Timetables.RouteId=Routes.Id ORDER BY Routes.RouteAbbreviation";

      var timetableList =
        SQLiteData.LoadData<TimetableRouteModel, dynamic>(sql, new {}, SQLiteData.GetConnectionString()).ToList();
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
                    @ServiceDirectionId, @IsMultiDirection, @RouteId);SELECT last_insert_rowid();";
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

		public static void DeleteTimetable(int timetableId)
			{
      string sql = "PRAGMA foreign_keys = ON;DELETE FROM Timetables WHERE Timetables.Id=@TimetableId;";
      SQLiteData.SaveData<dynamic>(sql, new { timetableId }, SQLiteData.GetConnectionString());

			}
		}
  }
