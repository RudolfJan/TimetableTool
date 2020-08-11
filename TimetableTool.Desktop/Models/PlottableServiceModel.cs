using DataAccess.Library.Models;
using ScottPlot;
using System.Collections.Generic;
using System.Drawing;

namespace TimetableTool.Desktop.Models
	{
	public class PlottableServiceModel : ExtendedServiceModel
		{
		public List<DataPoint> DataLine { get; set; } = new List<DataPoint>();
		public double[] LocationValue;
		public double[] TimeValue;
		public Color LineColor;
		public PlottableScatter Plot { get; set; }

		public PlottableServiceModel(ExtendedServiceModel service)
			{
			ServiceTemplateId = service.ServiceTemplateId;
			Id = service.Id;
			ServiceName = service.ServiceName;
			ServiceAbbreviation = service.ServiceAbbreviation;
			StartTime = service.StartTime;
			EndTime = service.EndTime;
			ServiceType = service.ServiceType;
			Category = service.Category;
			StartLocationId = service.StartLocationId;
			StartLocationName = service.StartLocationName;
			EndLocationId = service.EndLocationId;
			EndLocationName = service.EndLocationName;
			}
		}
	}
