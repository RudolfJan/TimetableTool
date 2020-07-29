using Caliburn.Micro;
using DataAccess.Library.Models;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TimetableTool.Desktop.Models
	{
	public class TimeGraphUIModel
		{
		public BindableCollection<FullTimeEventModel> TimeEventList { get; set; }= new BindableCollection<FullTimeEventModel>();
		public string ServiceAbbreviation { get; set; } = "";
		public string ServiceName { get; set; } = "";
		public string ServiceType { get; set; } = ""; // serviceClass
		// TODO split this off, the next parameter is only used in ScottPlot
		public PlottableScatter Plot { get; set; } // remember the dataset for the plot
		public BindableCollection<DataPoint> DataLine { get; set; } = new BindableCollection<DataPoint>();
		}
	}
