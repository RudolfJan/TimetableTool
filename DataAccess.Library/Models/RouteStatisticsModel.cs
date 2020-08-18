using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Library.Models
	{
	public class RouteStatisticsModel
		{
		public int Id { get; set; }
		public string RouteName { get; set; }
		public string RouteAbbreviation { get; set; }
		public int ServiceCount { get; set; }
		public int ServiceTemplateCount { get; set; }
		public int TrainCount { get; set; }
		public int LocationCount { get; set; }
		}
	}
