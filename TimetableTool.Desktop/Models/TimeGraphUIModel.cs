using Caliburn.Micro;
using DataAccess.Library.Models;
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
		public string ServiceInstanceAbbreviation { get; set; } = "";
		public string ServiceInstanceName { get; set; } = "";
		public BindableCollection<DataPoint> DataLine { get; set; } = new BindableCollection<DataPoint>();
		}
	}
