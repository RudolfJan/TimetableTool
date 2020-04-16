using System;
using System.Collections.Generic;
using System.Text;
using TimetableTool.DataAccessLibrary.Logic;

namespace DataAccess.Library.Models
	{
	public class ServiceInstanceModel
		{
		public int Id { get; set; }
		public string ServiceInstanceName { get; set; }
		public string ServiceInstanceAbbreviation { get; set; }
		public int StartTime { get; set; }
		public int EndTime { get; set; }
		public string StartTimeText
			{
			get
				{
				return TimeConverters.MinutesToString(StartTime);
				}
			}
		public string EndTimeText
			{
			get
				{
				return TimeConverters.MinutesToString(EndTime);
				}
			}
		public int ServiceId { get; set; }
		}
	}
