using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Library.Models
	{
	public class ServiceTemplateModel
		{
		public int Id { get; set; }
		public string ServiceTemplateName { get; set; }
		public string ServiceTemplateAbbreviation { get; set; }
		public string ServiceTemplateDescription { get; set; }
		public string ServiceType { get; set; }
		public int ServiceDirectionId { get; set; }
		public int CalculatedDuration { get; set; }
		public int RouteId { get; set; }
		}
	}
