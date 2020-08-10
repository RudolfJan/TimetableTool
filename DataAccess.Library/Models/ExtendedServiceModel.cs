using System;
using System.Collections.Generic;
using System.Text;
using TimetableTool.DataAccessLibrary.Logic;

namespace DataAccess.Library.Models
	{
	// Includes some items from ServiceTemplate class
	public class ExtendedServiceModel : ServiceModel
		{
			public string ServiceType { get; set; }
			public string Category { get; set; }

		// and the first and last location
		public int StartLocationId { get; set; }
		public string StartLocationName { get; set; }
		public int EndLocationId { get; set; }
		public string EndLocationName { get; set; }
			}
		}
	