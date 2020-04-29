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
    public static List<ServiceInstanceModel> GetServiceInstancesPerRoute(int routeId)
      {
      string sql = @"SELECT
                        ServiceInstances.Id AS Id,
                        ServiceInstances.ServiceInstanceName,
                        ServiceInstances.ServiceInstanceAbbreviation,
                        ServiceInstances.StartTime,
                        ServiceInstances.EndTime,
                        ServiceInstances.ServiceId
                      FROM ServiceInstances, Services WHERE @RouteId=Services.RouteId AND ServiceInstances.ServiceId== Services.Id";

      var serviceInstanceList =
        SQLiteData.LoadData<ServiceInstanceModel, dynamic>(sql, new { routeId}, SQLiteData.GetConnectionString()).ToList();
      return serviceInstanceList;
      }

   public static List<ServiceInstanceModel> GetServiceInstancesPerRoute(int routeId, int serviceDirectionId)
      {
      string sql = @"SELECT
                        ServiceInstances.Id AS Id,
                        ServiceInstances.ServiceInstanceName,
                        ServiceInstances.ServiceInstanceAbbreviation,
                        ServiceInstances.StartTime,
                        ServiceInstances.EndTime,
                        ServiceInstances.ServiceId
                      FROM ServiceInstances, Services 
                      WHERE 
                              @RouteId=Services.RouteId 
                              AND ServiceInstances.ServiceId== Services.Id 
                              AND Services.ServiceDirectionId=@ServiceDirectionId;";

      var serviceInstanceList =
        SQLiteData.LoadData<ServiceInstanceModel, dynamic>(sql, new { routeId, serviceDirectionId}, SQLiteData.GetConnectionString()).ToList();
      return serviceInstanceList;
      }


   public static int GetServiceDirectionByServiceInstanceId(int serviceInstanceId)
      {
      string sql = @"SELECT 
                         Services.ServiceDirectionId 
                    FROM Services, ServiceInstances 
                    WHERE 
                        Services.Id=ServiceInstances.ServiceId 
                        AND ServiceInstances.Id=@ServiceInstanceId;";

      var serviceDirectionId =
        SQLiteData.LoadData<int, dynamic>(sql, new { serviceInstanceId}, SQLiteData.GetConnectionString()).FirstOrDefault();
      return serviceDirectionId;
      }



    public static List<ServiceInstanceModel> GetServiceInstancesPerTimetable(int timetableId)
      {
      string sql = @" SELECT ServiceInstances.Id AS Id,
                        ServiceInstances.ServiceInstanceName,
                        ServiceInstances.ServiceInstanceAbbreviation,
                        ServiceInstances.StartTime,
                        ServiceInstances.EndTime,
                        ServiceInstances.ServiceId
                      FROM ServiceInstances, ConnectTtSi 
                      WHERE ConnectTtSi.TimetableId=@TimetableId
                            AND  ServiceInstances.Id=ConnectTtSi.ServiceInstanceId; ";

      var serviceInstanceList =
        SQLiteData.LoadData<ServiceInstanceModel, dynamic>(sql, new {timetableId}, SQLiteData.GetConnectionString()).ToList();
      return serviceInstanceList;
      }


    public static ServiceInstanceModel GetServiceInstanceById(int serviceInstanceId)
      {
      string sql = "SELECT * FROM ServiceInstances WHERE Id= @serviceInstanceId";

      var timeTable =
        SQLiteData.LoadData<ServiceInstanceModel, dynamic>(sql, new {serviceInstanceId}, SQLiteData.GetConnectionString()).FirstOrDefault();
      return timeTable;
      }

    public static void InsertServiceInstance(ServiceInstanceModel serviceInstance)
      {
      string sql = @"INSERT OR IGNORE INTO ServiceInstances (ServiceInstanceName, ServiceInstanceAbbreviation, StartTime, EndTime, ServiceId) 
                                    VALUES(@ServiceInstanceName, @ServiceInstanceAbbreviation, @StartTime, @EndTime, @ServiceId);";
      SQLiteData.SaveData<dynamic>(sql,
        new {serviceInstance.ServiceInstanceName, serviceInstance.ServiceInstanceAbbreviation, 
          serviceInstance.StartTime, serviceInstance.EndTime, serviceInstance.ServiceId}, 
        SQLiteData.GetConnectionString());
      }

    public static void UpdateServiceInstance(ServiceInstanceModel serviceInstance)
      {
      string sql = "UPDATE OR IGNORE ServiceInstances SET ServiceInstanceName=@ServiceInstanceName, ServiceInstanceAbbreviation=@ServiceInstanceAbbreviation, StartTime=@StartTime, EndTime=@EndTime, ServiceId=@ServiceId  WHERE Id=@Id";
      SQLiteData.SaveData<dynamic>(sql,
        new {serviceInstance.ServiceInstanceName, serviceInstance.ServiceInstanceAbbreviation, 
          serviceInstance.StartTime, serviceInstance.EndTime, serviceInstance.ServiceId, serviceInstance.Id}, 
        SQLiteData.GetConnectionString());
      }
    }
  }
