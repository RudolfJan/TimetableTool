using System;
using System.Collections.Generic;
using System.Text;
using TimetableTool.DataAccessLibrary.Logic;

namespace DataAccess.Library.Models
	{
	public class DepartureArrivalTimetableModel
		{
		public int ServiceId { get; set; }
		public string ServiceName { get; set; }
		public string ServiceAbbrev { get; set; }
		public string LocationName { get; set; }
		public string LocationAbbrev { get; set; }
		public string DestinationName { get; set; }
		public string DestinationAbbrev { get; set; }
		public string OriginName { get; set; }
		public string OriginAbbrev { get; set; }
		public int StartTime { get; set; }

		public string StartTimeText
			{
			get
				{
				return TimeConverters.MinutesToString(StartTime);
				}
			}
		public int EndTime { get; set; }

		public string EndTimeText
			{
			get
				{
				return TimeConverters.MinutesToString(EndTime);
				}
			}
		public int ArrivalTime { get; set; }
		public int DepartureTime { get; set; }
		public string EventType { get; set; }
		public string ArrivalTimeText
			{
			get
				{
				return TimeConverters.MinutesToString(ArrivalTime);
				}
			}
		public string DepartureTimeText
			{
			get
				{
				return TimeConverters.MinutesToString(DepartureTime);
				}
			}
		}
	}
