using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Library.Models
	{
	public class TrainModel
		{
		public int Id { get; set; }
		public string TrainName { get; set; }
		public string TrainAbbreviation { get; set; }
		public string TrainDescription { get; set; }
		public string TrainClass { get; set; }
		public int RouteId { get; set; }
		}
	}
