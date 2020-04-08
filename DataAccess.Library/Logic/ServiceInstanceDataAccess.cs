using DataAccess.Library.Models;
using SQLiteDataAccess.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Library.Logic
  {
  public class ServiceInstanceDataAccess
    {
    public static List<ServiceInstanceModel> GetServiceInstancesPerTimetable(int timetableId)
      {
      string sql = "SELECT * FROM ServiceInstances WHERE TimetableId=@TimetableId";

      var serviceInstanceList =
        SQLiteData.LoadData<ServiceInstanceModel, dynamic>(sql, new { timetableId}, SQLiteData.GetConnectionString()).ToList();
      return serviceInstanceList;
      }

    public static ServiceInstanceModel GetServiceInstanceById(int serviceInstanceId)
      {
      string sql = "SELECT * FROM ServiceInstances WHERE Id= @serviceInstanceId";

      var timeTable =
        SQLiteData.LoadData<ServiceInstanceModel, dynamic>(sql, new {serviceInstanceId}, SQLiteData.GetConnectionString()).FirstOrDefault();
      return timeTable;
      }

    public static int InsertServiceInstance(ServiceInstanceModel serviceInstance)
      {
      string sql = "INSERT OR IGNORE INTO ServiceInstances (ServiceInstanceName, ServiceInstanceAbbreviation, StartTime, EndTime, ServiceId, TimetableId) VALUES(@ServiceInstanceName, @ServiceInstanceAbbreviation, @StartTime, @EndTime, @ServiceId, @TimetableId)";
      return SQLiteData.SaveData<dynamic>(sql,
        new {serviceInstance.ServiceInstanceName, serviceInstance.ServiceInstanceAbbreviation, 
          serviceInstance.StartTime, serviceInstance.EndTime, serviceInstance.ServiceId, serviceInstance.TimetableId}, 
        SQLiteData.GetConnectionString());
      }

    public static void UpdateServiceInstance(ServiceInstanceModel serviceInstance)
      {
      string sql = "UPDATE OR IGNORE ServiceInstances SET ServiceInstanceName=@ServiceInstanceName, ServiceInstanceAbbreviation=@ServiceInstanceAbbreviation, StartTime=@StartTime, EndTime=@EndTime, ServiceId=@ServiceId, TimetableId=@TimetableId)  WHERE Id=@Id";
      SQLiteData.SaveData<dynamic>(sql,
        new {serviceInstance.ServiceInstanceName, serviceInstance.ServiceInstanceAbbreviation, 
          serviceInstance.StartTime, serviceInstance.EndTime, serviceInstance.ServiceId, serviceInstance.TimetableId, serviceInstance.Id}, 
        SQLiteData.GetConnectionString());
      }
    }
  }
