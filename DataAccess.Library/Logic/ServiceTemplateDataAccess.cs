using DataAccess.Library.Models;
using SQLiteDataAccess.Library;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Library.Logic
  {
  public class ServiceTemplateDataAccess
    {
    public static List<ServiceTemplateModel> GetServiceTemplatesPerRoute(int routeId)
      {
      string sql = "SELECT * FROM ServiceTemplates WHERE RouteId=@RouteId";

      var serviceList =
        SQLiteData.LoadData<ServiceTemplateModel, dynamic>(sql, new { routeId}, SQLiteData.GetConnectionString()).ToList();
      return serviceList;
      }

    public static ServiceTemplateModel GetServiceTemplateById(int serviceTemplateId)
      {
      string sql = "SELECT * FROM ServiceTemplates WHERE Id= @serviceTemplateId";

      var timeTable =
        SQLiteData.LoadData<ServiceTemplateModel, dynamic>(sql, new {serviceTemplateId}, SQLiteData.GetConnectionString()).FirstOrDefault();
      return timeTable;
      }

    public static int InsertServiceTemplate(ServiceTemplateModel service)
      {
      string sql = @"INSERT OR IGNORE INTO ServiceTemplates 
                      (ServiceTemplateName, ServiceTemplateAbbreviation, ServiceTemplateDescription, 
                      ServiceType, ServiceDirectionId, CalculatedDuration, RouteId)
                      VALUES(@ServiceTemplateName, @ServiceTemplateAbbreviation, @ServiceTemplateDescription, 
                      @ServiceType, @ServiceDirectionId, @CalculatedDuration, @RouteId);SELECT last_insert_rowid();";
      return SQLiteData.SaveData<dynamic>(sql,
        new {service.ServiceTemplateName, service.ServiceTemplateAbbreviation, service.ServiceTemplateDescription, 
                      service.ServiceType, service.ServiceDirectionId, service.CalculatedDuration, service.RouteId}, 
                      SQLiteData.GetConnectionString());
      }

    public static void UpdateServiceTemplate(ServiceTemplateModel service)
      {
      string sql = @"UPDATE OR IGNORE ServiceTemplates 
                  SET 
                        ServiceTemplateName=@ServiceTemplateName, 
                        ServiceTemplateAbbreviation=@ServiceTemplateAbbreviation, 
                        ServiceTemplateDescription=@ServiceTemplateDescription, 
                        ServiceType=@ServiceType, 
                        ServiceDirectionId=@ServiceDirectionId, 
                        CalculatedDuration=@CalculatedDuration, 
                        RouteId=@RouteId 
                  WHERE Id=@Id";
      SQLiteData.SaveData<dynamic>(sql,
        new {service.ServiceTemplateName, service.ServiceTemplateAbbreviation, service.ServiceTemplateDescription, 
          service.ServiceType, service.ServiceDirectionId, service.CalculatedDuration, service.RouteId, service.Id}, 
        SQLiteData.GetConnectionString());
      }

    public static void UpdateServiceCalculatedDuration(int calculatedDuration, int serviceId)
      {
      string sql = "UPDATE OR IGNORE ServiceTemplates SET CalculatedDuration=@CalculatedDuration WHERE Id=@ServiceId";
      SQLiteData.SaveData<dynamic>(sql,
        new {calculatedDuration, serviceId}, SQLiteData.GetConnectionString());
      }

		public static void DeleteServiceTemplate(int serviceTemplateId)
			{
      string sql = "PRAGMA foreign_keys = ON;DELETE FROM ServiceTemplates WHERE ServiceTemplates.Id=@ServiceTemplateId;";
      SQLiteData.SaveData<dynamic>(sql, new { serviceTemplateId }, SQLiteData.GetConnectionString());
			}
		}
  }
