using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Library.Models
	{

	/*
	SELECT ServiceTemplates.Id AS Id, Routes.RouteName, Routes.RouteAbbreviation, Routes.Id
	, ServiceTemplates.ServiceTemplateName, ServiceTemplates.ServiceTemplateAbbreviation, ServiceTemplates.Id, COUNT(*) As ServiceCount
	FROM Services, ServiceTemplates, Routes, 
	WHERE Services.ServiceTemplateId= ServiceTemplates.Id AND ServiceTemplates.RouteId= Routes.Id 
	GROUP BY ServiceTemplates.ServiceTemplateName;

		*/
	public class ServiceTemplateStatisticsModel
		{
		public int Id { get; set; }
		public string ServiceTemplateName { get; set; }
		public string ServiceTemplateAbbreviation { get; set; }
		public string ServiceTemplateDescription { get; set; }
		public int RouteId { get; set; }
		public string RouteName { get; set; }
		public string RouteAbbreviation { get; set; }
		public int ServiceCount { get; set; }
		public int TimeEventCount { get; set; }
		public int ServicesInTrainCount { get; set; }
		}
	}
