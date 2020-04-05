using DataAccess.Library.Models;
using SQLiteDataAccess.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Library.Logic
  {
  public class TimeTableDataAccess
    {
    public static List<TimeTableModel> GetAllTimeTablesPerRoute(int routeId)
      {
      string sql = "SELECT * FROM TimeTables WHERE RouteId=@RouteId";

      var timeTableList =
        SQLiteData.LoadData<TimeTableModel, dynamic>(sql, new { routeId}, SQLiteData.GetConnectionString()).ToList();
      return timeTableList;
      }

    public static TimeTableModel GetTimeTableById(int timeTableId)
      {
      string sql = "SELECT * FROM TimeTables WHERE Id= @timeTableId";

      var timeTable =
        SQLiteData.LoadData<TimeTableModel, dynamic>(sql, new {timeTableId}, SQLiteData.GetConnectionString()).FirstOrDefault();
      return timeTable;
      }

    public static int InsertLocationForRoute(TimeTableModel timteTable)
      {
      string sql = "INSERT OR IGNORE INTO TimeTables (TimeTableName, TimeTableAbbreviation, TimeTableDescription, RouteId) VALUES(@TimeTableName, @TimeTableAbbreviation, @TimeTableDescription, @RouteId)";
      return SQLiteData.SaveData<dynamic>(sql,new {timteTable.TimeTableName, timteTable.TimeTableAbbreviation, timteTable.TimeTableDescription, timteTable.RouteId}, SQLiteData.GetConnectionString());
      }
    }
  }
