using DataAccess.Library.Models;
using SQLiteDataAccess.Library;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Library.Logic
  {
  public class ServiceDataAccess
    {
    public static List<ServiceModel> GetServicesPerRoute(int routeId)
      {
      string sql = "SELECT * FROM Services WHERE RouteId=@RouteId";

      var serviceList =
        SQLiteData.LoadData<ServiceModel, dynamic>(sql, new { routeId}, SQLiteData.GetConnectionString()).ToList();
      return serviceList;
      }

    public static ServiceModel GetServiceById(int serviceId)
      {
      string sql = "SELECT * FROM Services WHERE Id= @serviceId";

      var timeTable =
        SQLiteData.LoadData<ServiceModel, dynamic>(sql, new {serviceId}, SQLiteData.GetConnectionString()).FirstOrDefault();
      return timeTable;
      }

    public static int InsertServiceForRoute(ServiceModel service)
      {
      string sql = @"INSERT OR IGNORE INTO Services 
                      (ServiceName, ServiceAbbreviation, ServiceDescription, 
                      ServiceType, ServiceDirectionId, CalculatedDuration, RouteId)
                      VALUES(@ServiceName, @ServiceAbbreviation, @ServiceDescription, 
                      @ServiceType, @ServiceDirectionId, @CalculatedDuration, @RouteId);SELECT last_insert_rowid();";
      return SQLiteData.SaveData<dynamic>(sql,
        new {service.ServiceName, service.ServiceAbbreviation, service.ServiceDescription, 
                      service.ServiceType, service.ServiceDirectionId, service.CalculatedDuration, service.RouteId}, 
                      SQLiteData.GetConnectionString());
      }

    public static void UpdateService(ServiceModel service)
      {
      string sql = @"UPDATE OR IGNORE Services 
                  SET 
                        ServiceName=@ServiceName, 
                        ServiceAbbreviation=@ServiceAbbreviation, 
                        ServiceDescription=@ServiceDescription, 
                        ServiceType=@ServiceType, 
                        ServiceDirectionId=@ServiceDirectionId, 
                        CalculatedDuration=@CalculatedDuration, 
                        RouteId=@RouteId 
                  WHERE Id=@Id";
      SQLiteData.SaveData<dynamic>(sql,
        new {service.ServiceName, service.ServiceAbbreviation, service.ServiceDescription, 
          service.ServiceType, service.ServiceDirectionId, service.CalculatedDuration, service.RouteId, service.Id}, 
        SQLiteData.GetConnectionString());
      }

    public static void UpdateServiceCalculatedDuration(int calculatedDuration, int serviceId)
      {
      string sql = "UPDATE OR IGNORE Services SET CalculatedDuration=@CalculatedDuration WHERE Id=@ServiceId";
      SQLiteData.SaveData<dynamic>(sql,
        new {calculatedDuration, serviceId}, SQLiteData.GetConnectionString());
      }

		public static void DeleteService(int serviceId)
			{
      string sql = "PRAGMA foreign_keys = ON;DELETE FROM Services WHERE Services.Id=@ServiceId;";
      SQLiteData.SaveData<dynamic>(sql, new { serviceId }, SQLiteData.GetConnectionString());
			}
		}
  }
