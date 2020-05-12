using TimetableTool.DataAccessLibrary.Logic;

namespace DataAccess.Library.Models
	{
	public class ServiceModel
		{
		public int Id { get; set; }
		public string ServiceName { get; set; }
		public string ServiceAbbreviation { get; set; }
		public int StartTime { get; set; }
		public int EndTime { get; set; }
		public int ServiceTemplateId { get; set; }
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

		}
	}
