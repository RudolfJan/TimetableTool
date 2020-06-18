using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Library.Models
	{
	public class TimeEventTypeModel
		{
		public int Id { get; set; }
		public string EventType { get; set; }
		public string EventTypeDescription { get; set; }

		// https://rachel53461.wordpress.com/2011/08/20/comboboxs-selecteditem-not-displaying/
		// Needed to properly display the combobox
		public override bool Equals(object obj)
			{
			if (obj == null || !(obj is TimeEventTypeModel))
				return false;
			return ((TimeEventTypeModel)obj).EventType == this.EventType;
			}
		}
	}
