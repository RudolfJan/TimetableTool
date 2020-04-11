using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Library.Models
	{
	public class ServiceInstanceTimingModel
		{
		//Locations.Id as LocationId, 
		//TimeEvents.id as TimeEventId, 
		//Locations.LocationName as LocationName,
		//Locations.LocationAbbreviation as LocationAbbrev,
		//TimeEvents.EventType as EventType,
		//TimeEvents.RelativeTime as RelativeTime

		public int LocationId { get; set; }
		public int TimeEventId { get; set; }
		public string LocationName { get; set; }
		public string LocationAbbrev { get; set; }
		public string EventType { get; set; }
		public int RelativeTime { get; set; }
		public int LocationsOrder { get; set; }
		public string TimeString { get; set; }
		}
	}
