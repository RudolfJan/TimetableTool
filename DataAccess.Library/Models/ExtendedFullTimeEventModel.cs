using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Library.Models
	{
	public class ExtendedFullTimeEventModel: FullTimeEventModel
		{
		// FullTimeEventModel, but the model is extended withe arrival and departure time. These tumes are absolute, so only use this for services ...
		public string ArrivalTimeText { get; set; }
		public string DepartureTimeText { get; set; }
		}
	}
