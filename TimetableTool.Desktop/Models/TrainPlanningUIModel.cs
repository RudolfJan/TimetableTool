using DataAccess.Library.Models;
using ScottPlot;
using System.Collections.Generic;
using System.Drawing;

namespace TimetableTool.Desktop.Models
	{
	public class TrainPlanningUIModel
		{
		public TrainModel Train { get; set; }
		public List<PlottableServiceModel> ServicesInTrain { get; set; }
		public DataPoint LegendDataPoint { get; set; } = null;
		}
	}
