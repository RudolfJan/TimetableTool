using DataAccess.Library.Models;
using SQLiteDataAccess.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Library.Logic
	{
	public class TimeEventTypeDataAccess
		{
		public static List<TimeEventTypeModel> GetAllTimeEventTypes()
			{
			var sql="SELECT Id, EventType, EventTypeDescription FROM \"TimeEventTypes\";";
			var timeEventList =
				SQLiteData.LoadData<TimeEventTypeModel, dynamic>(sql, new {}, SQLiteData.GetConnectionString()).ToList();
			return timeEventList;
			}

		public static List<string> GetAllTimeEventTypeStrings()
			{
			var sql="SELECT Id, EventType, EventTypeDescription FROM \"TimeEventTypes\";";
			var timeEventList =
				SQLiteData.LoadData<TimeEventTypeModel, dynamic>(sql, new {}, SQLiteData.GetConnectionString()).ToList();
			var output = timeEventList.Select(x => x.EventType).ToList();
			return output;
			}

		public static TimeEventTypeModel GetTimeEventTypeModelFromString(string timeEventType, List<TimeEventTypeModel> timeEventList)
			{
			TimeEventTypeModel output = timeEventList.First(x => x.EventType == timeEventType);
			if (output == null)
				{
				TimeEventTypeModel defaultValue= new TimeEventTypeModel();
				defaultValue.EventType = "??";
				defaultValue.EventTypeDescription = "Choose from the predefined list";
				output = defaultValue;
				}
			return output;
			}
		}
	}
