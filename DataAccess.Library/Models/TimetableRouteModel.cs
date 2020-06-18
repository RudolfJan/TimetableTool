using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Library.Models
	{
	public class TimetableRouteModel
		{
		public int TimetableId { get; set; }
		public int RouteId { get; set; }
		public string RouteAbbreviation { get; set; }
		public string RouteName { get; set; }
		public string TimetableAbbreviation { get; set; }
		public string TimetableName { get; set; }
		public string TimetableDescription { get; set; }
		public int ServiceDirectionId { get; set; }
		public bool IsMultiDirection { get; set; }
		}
	}
