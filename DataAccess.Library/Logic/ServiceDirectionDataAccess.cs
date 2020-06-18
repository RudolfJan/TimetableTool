using DataAccess.Library.Models;
using SQLiteDataAccess.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Library.Logic
	{
	public class ServiceDirectionDataAccess
		{
		  public static List<ServiceDirectionModel> GetAllServiceDirectionsPerRoute(int routeId)
      {
      string sql = "SELECT * FROM ServiceDirections WHERE RouteId=@RouteId";

      var serviceDirectionList =
        SQLiteData.LoadData<ServiceDirectionModel, dynamic>(sql, new { routeId}, SQLiteData.GetConnectionString()).ToList();
      return serviceDirectionList;
      }

		internal static ServiceDirectionModel GetServiceDirectionByServiceTemplateId(int serviceTemplateId)
			{
			string sql = "SELECT ServiceDirections.Id, ServiceDirections.ServiceDirectionName, ServiceDirections.ServiceDirectionAbbreviation, ServiceDirections.RouteId, ServiceDirections.IsDescending " +
			             "FROM ServiceDirections, ServiceTemplates WHERE ServiceTemplates.id=@ServiceTemplateId AND ServiceDirections.Id= ServiceTemplates.ServiceDirectionId";
			var serviceDirection =
				SQLiteData.LoadData<ServiceDirectionModel, dynamic>(sql, new { serviceTemplateId}, SQLiteData.GetConnectionString()).FirstOrDefault();
			return serviceDirection;
			}

		public static ServiceDirectionModel GetServiceDirectionById(int serviceDirectionId)
      {
      string sql = "SELECT * FROM ServiceDirections WHERE Id= @ServiceDirectionId";

      var serviceDirection =
        SQLiteData.LoadData<ServiceDirectionModel, dynamic>(sql, new {serviceDirectionId}, SQLiteData.GetConnectionString()).FirstOrDefault();
      return serviceDirection;
      }

    public static int InsertServiceDirection(ServiceDirectionModel serviceDirection)
      {
      string sql = "INSERT OR IGNORE INTO ServiceDirections (ServiceDirectionName, ServiceDirectionAbbreviation,  RouteId, IsDescending) " +
                   "VALUES(@ServiceDirectionName, @ServiceDirectionAbbreviation, @RouteId, @IsDescending);" +
                   "SELECT last_insert_rowid();";
      return SQLiteData.SaveData<dynamic>(sql,new {serviceDirection.ServiceDirectionName, serviceDirection.ServiceDirectionAbbreviation, serviceDirection.RouteId, serviceDirection.IsDescending}, SQLiteData.GetConnectionString());
      }

    public static void UpdateServiceDirectionForRoute(ServiceDirectionModel serviceDirection)
      {
      string sql = "UPDATE OR IGNORE ServiceDirections SET ServiceDirectionName=@ServiceDirectionName, serviceDirectionAbbreviation=@ServiceDirectionAbbreviation, RouteId=@RouteId, IsDescending=@IsDescending WHERE Id=@Id";
      SQLiteData.SaveData<dynamic>(sql,new {serviceDirection.Id, serviceDirection.ServiceDirectionName, serviceDirection.ServiceDirectionAbbreviation, serviceDirection.RouteId, serviceDirection.IsDescending}, SQLiteData.GetConnectionString());
      }

		public static void DeleteServiceDirection(int serviceDirectionId)
			{
			string sql= "PRAGMA foreign_keys = ON;DELETE FROM ServiceDirections WHERE ServiceDirections.Id=@ServiceDirectionId;";
			SQLiteData.SaveData<dynamic>(sql,new {serviceDirectionId}, SQLiteData.GetConnectionString());
			}
		}
	}
