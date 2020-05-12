using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Library.Models
	{
	public class FullServiceModel
		{
		public int Id { get; set; }
		public int ServiceId { get; set; }
		public int ServiceTemplateId { get; set; }
		public int TimetableId { get; set; }
		public string ServiceName { get; set; }
		public string ServiceAbbreviation { get; set; }
		public int StartTime { get; set; }
		public int EndTime { get; set; }



		}
	}
