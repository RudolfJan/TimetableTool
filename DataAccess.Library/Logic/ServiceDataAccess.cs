﻿using DataAccess.Library.Models;
using SQLiteDataAccess.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
      string sql = "INSERT OR IGNORE INTO Services (ServiceName, ServiceAbbreviation, ServiceDescription, ServiceType, CalculatedDuration,RouteId) VALUES(@ServiceName, @ServiceAbbreviation, @ServiceDescription, @ServiceType, @CalculatedDuration, @RouteId)";
      return SQLiteData.SaveData<dynamic>(sql,
        new {service.ServiceName, service.ServiceAbbreviation, service.ServiceDescription, 
                      service.ServiceType, service.CalculatedDuration, service.RouteId}, 
                      SQLiteData.GetConnectionString());
      }
    }
  }
