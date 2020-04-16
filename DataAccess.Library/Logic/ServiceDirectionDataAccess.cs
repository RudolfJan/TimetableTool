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

    public static ServiceDirectionModel GetServiceDirectionById(int serviceDirectionId)
      {
      string sql = "SELECT * FROM ServiceDirections WHERE Id= @ServiceDirectionId";

      var serviceDirection =
        SQLiteData.LoadData<ServiceDirectionModel, dynamic>(sql, new {serviceDirectionId}, SQLiteData.GetConnectionString()).FirstOrDefault();
      return serviceDirection;
      }

    public static int InsertServiceDirection(ServiceDirectionModel serviceDirection)
      {
      string sql = "INSERT OR IGNORE INTO ServiceDirections (ServiceDirectionName, ServiceDirectionAbbreviation,  RouteId) VALUES(@ServiceDirectionName, @ServiceDirectionAbbreviation, @RouteId)";
      return SQLiteData.SaveData<dynamic>(sql,new {serviceDirection.ServiceDirectionName, serviceDirection.ServiceDirectionAbbreviation, serviceDirection.RouteId}, SQLiteData.GetConnectionString());
      }

    public static void UpdateServiceDirectionForRoute(ServiceDirectionModel serviceDirection)
      {
      string sql = "UPDATE OR IGNORE ServiceDirections SET ServiceDirectionName=@ServiceDirectionName, serviceDirectionAbbreviation=@ServiceDirectionAbbreviation, RouteId=@RouteId WHERE Id=@Id";
      SQLiteData.SaveData<dynamic>(sql,new {serviceDirection.ServiceDirectionName, serviceDirection.ServiceDirectionAbbreviation, serviceDirection.RouteId, serviceDirection.Id}, SQLiteData.GetConnectionString());
      }
		}
	}
