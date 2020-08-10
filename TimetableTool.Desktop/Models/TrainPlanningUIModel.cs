using DataAccess.Library.Models;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TimetableTool.Desktop.Models
	{
	public class TrainPlanningUIModel
		{
		public TrainModel Train { get; set; }
		public List<ExtendedServiceModel> ServicesInTrain { get; set; }
		public List<DataPoint> DataLine { get; set; } = new List<DataPoint>();
		public double[] LocationValue;
		public double[] TimeValue;
		public Color[] LineColor;
		public PlottableScatter Plot { get; set; } // remember the data set for the plot
		}
	}
