using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Library.Models
	{
	public class ServiceInstanceTimingModel
		{
		public int LocationId { get; set; }
		public int TimeEventId { get; set; }
		public string LocationName { get; set; }
		public string LocationAbbrev { get; set; }
		public string EventType { get; set; }
		public int ArrivalTime { get; set; }
		public int WaitTime { get; set; }
		public int LocationsOrder { get; set; }
		public string TimeString { get; set; }
		}
	}
