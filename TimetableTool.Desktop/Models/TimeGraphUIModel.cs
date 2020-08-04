using Caliburn.Micro;
using DataAccess.Library.Models;
using Microsoft.Extensions.Primitives;
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
		public BindableCollection<ExtendedFullTimeEventModel> TimeEventList { get; set; }= new BindableCollection<ExtendedFullTimeEventModel>();
		public string ServiceAbbreviation { get; set; } = "";
		public string ServiceName { get; set; } = "";
		public string ServiceType { get; set; } = ""; // serviceClass

		public string StartTimeText { get; set; } = "";
		public string EndTimeText { get; set; } = "";

		// TODO split this off, the next parameter is only used in ScottPlot
		public PlottableScatter Plot { get; set; } // remember the dataset for the plot
		public BindableCollection<DataPoint> DataLine { get; set; } = new BindableCollection<DataPoint>();
		}
	}
