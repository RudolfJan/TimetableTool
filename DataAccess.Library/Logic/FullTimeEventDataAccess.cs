using DataAccess.Library.Models;
using SQLiteDataAccess.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Library.Logic
  {
  public class FullTimeEventDataAccess
    {
    public static List<FullTimeEventModel> GetAllFullTimeEventsPerServiceTemplate(int serviceTemplateId)
      {
      string sql = "SELECT * FROM FullTimeEvents " +
                   "WHERE ServiceTemplateId=@ServiceTemplateId";

      var timeEventList =
        SQLiteData.LoadData<FullTimeEventModel, dynamic>(sql, new { serviceTemplateId}, SQLiteData.GetConnectionString()).ToList();
      return timeEventList;
      }

    public static List<FullTimeEventModel> CreateTimeEventsPerServiceTemplate(int serviceTemplateId)
      {
      string sql ="SELECT ServiceDirections.IsDescending FROM ServiceDirections, ServiceTemplates " +
                  "WHERE ServiceTemplates.Id=@ServiceTemplateId AND ServiceDirections.Id=ServiceTemplates.ServiceDirectionId;";
      var isDescending= SQLiteData.LoadData<int, dynamic>(sql, new{serviceTemplateId},SQLiteData.GetConnectionString()).FirstOrDefault();
      string direction;

      if(isDescending==1)
        {
        direction="DESC";
        }
      else
        {
        direction="ASC";
        }

      sql = $"SELECT Locations.Id, Locations.LocationName, Locations.LocationAbbreviation, Locations.NumberOfTracks, Locations.[Order], Locations.RouteId FROM Locations, ServiceTemplates " +
            $"WHERE ServiceTemplates.Id=@ServiceTemplateId AND Locations.RouteId= ServiceTemplates.RouteId ORDER BY Locations.[Order] {direction};";

      var locations =
        SQLiteData.LoadData<LocationModel, dynamic>(sql, new { serviceTemplateId}, SQLiteData.GetConnectionString()).ToList();

      var timeEventList= new List<FullTimeEventModel>();
      int order =10;
      // Warning: Not all columns are filled!
      // TODO refactor this, using extraneous fields.
      foreach(var location in locations)
        {
        var timeEvent= new FullTimeEventModel();
        timeEvent.Order=order;
        order+=10;
        timeEvent.LocationId= location.Id;
        timeEvent.ServiceTemplateId= serviceTemplateId;
        timeEvent.LocationName= location.LocationName;
        timeEventList.Add(timeEvent);
        }
      return timeEventList;
      }

    public static FullTimeEventModel GetFullTimeEventById(int timeEventId)
      {
      string sql = "SELECT * FROM FullTimeEvents WHERE Id= @TimeEventId";

      var timeEvent =
        SQLiteData.LoadData<FullTimeEventModel, dynamic>(sql, new {timeEventId}, SQLiteData.GetConnectionString()).FirstOrDefault();
      return timeEvent;
      }

		public static List<ExtendedFullTimeEventModel> GetAllExtendedFullTimeEventsPerServiceTemplate(int serviceTemplateId)
			{
      string sql = "SELECT * FROM FullTimeEvents " +
                   "WHERE ServiceTemplateId=@ServiceTemplateId";

      var timeEventList =
        SQLiteData.LoadData<ExtendedFullTimeEventModel, dynamic>(sql, new { serviceTemplateId }, SQLiteData.GetConnectionString()).ToList();
      return timeEventList;
      }
		}
  }
