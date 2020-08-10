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

    public static List<ExtendedServiceModel> GetServicesPerRouteButNotInTrainServices(int routeId)
      {
      string sql = @"SELECT
                        Services.Id AS Id,
                        Services.ServiceName,
                        Services.ServiceAbbreviation,
                        Services.StartTime,
                        Services.EndTime,
                        Services.ServiceTemplateId,
                        ServiceTemplates.ServiceType AS ServiceType
                      FROM Services, ServiceTemplates WHERE ServiceTemplates.RouteId=@RouteId AND Services.ServiceTemplateId== ServiceTemplates.Id
                      AND Services.Id NOT IN (SELECT TrainServices.ServiceId FROM TrainServices)
                      ORDER BY Services.StartTime ASC";

      var serviceList =
        SQLiteData.LoadData<ExtendedServiceModel, dynamic>(sql, new { routeId }, SQLiteData.GetConnectionString()).ToList();

      //TODO need to make this more effective? we may need something like an extendeServiceTemplate, where now an extendedService is used...
      // Change this might increase efficiency quite a lot for this code.
      foreach(var service in serviceList)
        {
        var startLocation = GetSTartOrEndLocationForServiceTemplate(service.ServiceTemplateId,true);
        service.StartLocationId = startLocation.LocationId;
        service.StartLocationName = startLocation.LocationName;
        var endLocation = GetSTartOrEndLocationForServiceTemplate(service.ServiceTemplateId, false);
        service.EndLocationId = endLocation.LocationId;
        service.EndLocationName = endLocation.LocationName;
        }
      return serviceList;
      }

    private static LocationHelperModel GetSTartOrEndLocationForServiceTemplate(int serviceTemplateId, bool isStartLocation)
      {
      string requestType;
      if (isStartLocation)
        {
        requestType = "MIN";
        }
      else
        {
        requestType = "MAX";
        }
      string sql= @$"SELECT {requestType}(TimeEvents.[Order]) AS [Order], TimeEvents.LocationId, Locations.LocationName
      FROM TimeEvents, Locations
      WHERE TimeEvents.ServiceTemplateId = @serviceTemplateId
      AND Locations.Id == TimeEvents.LocationId;";
      LocationHelperModel location = SQLiteData.LoadData<LocationHelperModel,dynamic>(sql,new{serviceTemplateId}, SQLiteData.GetConnectionString()).First();
      return location;
      }

    public static List<ExtendedServiceModel> GetServicesPerRoutePerTrainInTrainServices(int routeId, int trainId)
      {
      string sql = @"SELECT
                        Services.Id AS Id,
                        Services.ServiceName,
                        Services.ServiceAbbreviation,
                        Services.StartTime,
                        Services.EndTime,
                        Services.ServiceTemplateId,
                        ServiceTemplates.ServiceType AS ServiceType
                      FROM Services, ServiceTemplates WHERE ServiceTemplates.RouteId=@RouteId AND Services.ServiceTemplateId== ServiceTemplates.Id
                      AND Services.Id IN (SELECT TrainServices.ServiceId FROM TrainServices WHERE TrainServices.TrainId= @TrainId)
                      ORDER BY Services.StartTime ASC;";

      var serviceList =
        SQLiteData.LoadData<ExtendedServiceModel, dynamic>(sql, new { routeId, trainId }, SQLiteData.GetConnectionString()).ToList();
      //TODO need to make this more effective? we may need something like an extendeServiceTemplate, where now an extendedService is used...
      // Change this might increase efficiency quite a lot for this code.
      foreach (var service in serviceList)
        {
        var startLocation = GetSTartOrEndLocationForServiceTemplate(service.ServiceTemplateId, true);
        service.StartLocationId = startLocation.LocationId;
        service.StartLocationName = startLocation.LocationName;
        var endLocation = GetSTartOrEndLocationForServiceTemplate(service.ServiceTemplateId, false);
        service.EndLocationId = endLocation.LocationId;
        service.EndLocationName = endLocation.LocationName;
        }
      return serviceList;
      }
    }
  }
