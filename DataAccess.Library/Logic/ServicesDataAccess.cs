using DataAccess.Library.Models;
using SQLiteDataAccess.Library;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Library.Logic
  {
  public class ServicesDataAccess
    {
    public static List<ServiceModel> GetServicesPerRoute(int routeId)
      {
      string sql = @"SELECT
                        Services.Id AS Id,
                        Services.ServiceName,
                        Services.ServiceAbbreviation,
                        Services.StartTime,
                        Services.EndTime,
                        Services.ServiceTemplateId
                      FROM Services, ServiceTemplates WHERE @RouteId=ServiceTemplates.RouteId AND Services.ServiceTemplateId== ServiceTemplates.Id";

      var serviceList =
        SQLiteData.LoadData<ServiceModel, dynamic>(sql, new { routeId}, SQLiteData.GetConnectionString()).ToList();
      return serviceList;
      }

    public static List<ExtendedServiceModel> GetExtendedServicesPerRoute(int routeId)
      {
      List<ServiceClassModel> serviceClasses = ServiceClassDataAccess.GetAllServiceClasses();

      string sql = @"SELECT
                        Services.Id AS Id,
                        Services.ServiceName,
                        Services.ServiceAbbreviation,
                        Services.StartTime,
                        Services.EndTime,
                        ServiceTemplates.ServiceType,
                        Services.ServiceTemplateId
                      FROM Services, ServiceTemplates
                      WHERE @RouteId=ServiceTemplates.RouteId 
                            AND Services.ServiceTemplateId== ServiceTemplates.Id
                          ";

      var serviceList =
        SQLiteData.LoadData<ExtendedServiceModel, dynamic>(sql, new { routeId }, SQLiteData.GetConnectionString()).ToList();
      foreach (var item in serviceList)
        {
        var classVar =
          ServiceClassDataAccess.GetServiceClassModelFromString(item.ServiceType, serviceClasses);
        item.Category = classVar.Category;
        }
      return serviceList;
      }

    public static List<ServiceModel> GetServicesPerRoute(int routeId, int serviceDirectionId)
      {

      string sql = @"SELECT
                        Services.Id AS Id,
                        Services.ServiceName,
                        Services.ServiceAbbreviation,
                        Services.StartTime,
                        Services.EndTime,
                        Services.ServiceTemplateId
                      FROM Services, ServiceTemplates 
                      WHERE 
                              @RouteId=ServiceTemplates.RouteId 
                              AND Services.ServiceTemplateId== ServiceTemplates.Id 
                              AND ServiceTemplates.ServiceDirectionId=@ServiceDirectionId;";

      var serviceList =
        SQLiteData.LoadData<ServiceModel, dynamic>(sql, new { routeId, serviceDirectionId}, SQLiteData.GetConnectionString()).ToList();
 
      return serviceList;
      }


    public static List<ExtendedServiceModel> GetExtendedServicesPerRoute(int routeId, int serviceDirectionId)
      {
      List<ServiceClassModel> serviceClasses = ServiceClassDataAccess.GetAllServiceClasses();
      string sql = @"SELECT
                        Services.Id AS Id,
                        Services.ServiceName,
                        Services.ServiceAbbreviation,
                        Services.StartTime,
                        Services.EndTime,
                        ServiceTemplates.ServiceType,
                        Services.ServiceTemplateId
                      FROM Services, ServiceTemplates
                      WHERE 
                              @RouteId=ServiceTemplates.RouteId 
                              AND Services.ServiceTemplateId== ServiceTemplates.Id 
                              AND ServiceTemplates.ServiceDirectionId=@ServiceDirectionId;";

      var serviceList =
        SQLiteData.LoadData<ExtendedServiceModel, dynamic>(sql, new { routeId, serviceDirectionId }, SQLiteData.GetConnectionString()).ToList();
      foreach (var item in serviceList)
        {
        var classVar =
          ServiceClassDataAccess.GetServiceClassModelFromString(item.ServiceType, serviceClasses);
        item.Category = classVar.Category;
        }
      return serviceList;
      }


    public static List<ServiceModel> GetServicesPerTimetable(int timetableId)
      {
      string sql = @" SELECT Services.Id AS Id,
                        Services.ServiceName,
                        Services.ServiceAbbreviation,
                        Services.StartTime,
                        Services.EndTime,
                        Services.ServiceTemplateId
                      FROM Services, ConnectTtSi 
                      WHERE ConnectTtSi.TimetableId=@TimetableId
                            AND  Services.Id=ConnectTtSi.ServiceId; ";

      var serviceList =
        SQLiteData.LoadData<ServiceModel, dynamic>(sql, new {timetableId}, SQLiteData.GetConnectionString()).ToList();
      return serviceList;
      }


    public static ServiceModel GetServiceById(int serviceId)
      {
      string sql = "SELECT * FROM Services WHERE Id= @serviceId";

      var timeTable =
        SQLiteData.LoadData<ServiceModel, dynamic>(sql, new {serviceId}, SQLiteData.GetConnectionString()).FirstOrDefault();
      return timeTable;
      }

    public static int InsertService(ServiceModel service)
      {
      string sql = @"INSERT OR IGNORE INTO Services (ServiceName, ServiceAbbreviation, StartTime, EndTime, ServiceTemplateId) 
                                    VALUES(@ServiceName, @ServiceAbbreviation, @StartTime, @EndTime, @ServiceTemplateId);SELECT last_insert_rowid();";
      return SQLiteData.SaveData<dynamic>(sql,
        new {service.ServiceName, service.ServiceAbbreviation, 
          service.StartTime, service.EndTime, service.ServiceTemplateId}, 
        SQLiteData.GetConnectionString());
      }

		public static void DeleteService(int serviceId)
			{
      string sql = "PRAGMA foreign_keys = ON;DELETE FROM Services WHERE Services.Id=@ServiceId;";
      SQLiteData.SaveData<dynamic>(sql, new { serviceId }, SQLiteData.GetConnectionString());
			}

		public static void UpdateService(ServiceModel service)
      {
      string sql = "UPDATE OR IGNORE Services SET ServiceName=@ServiceName, ServiceAbbreviation=@ServiceAbbreviation, StartTime=@StartTime, EndTime=@EndTime, ServiceTemplateId=@ServiceTemplateId  WHERE Id=@Id";
      SQLiteData.SaveData<dynamic>(sql,
        new {service.ServiceName, service.ServiceAbbreviation, 
          service.StartTime, service.EndTime, service.ServiceTemplateId, service.Id}, 
        SQLiteData.GetConnectionString());
      }
    }
  }
