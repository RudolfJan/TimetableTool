using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Library.Models
	{
	public class ServiceClassModel
		{
		public int Id { get; set; }
		public string ServiceClassName { get; set; }
		public string ServiceClassDescription { get; set; }
		public string Category { get; set; }

		// https://rachel53461.wordpress.com/2011/08/20/comboboxs-selecteditem-not-displaying/
		// Needed to properly display the combobox
		public override bool Equals(object obj)
			{
			if (obj == null || !(obj is ServiceClassModel))
				return false;

			return ((ServiceClassModel)obj).ServiceClassName == this.ServiceClassName;
			}
		}
	}
