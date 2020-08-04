using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Library.Models
	{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
	public class ServiceClassModel
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
		{
		public int Id { get; set; }
		public string ServiceClassName { get; set; }
		public string ServiceClassDescription { get; set; }
		public string Category { get; set; }
		public string Color { get; set; } = "Magenta"; // Ugly default color

		// https://rachel53461.wordpress.com/2011/08/20/comboboxs-selecteditem-not-displaying/
		// Needed to properly display the combobox
		public override bool Equals(object obj)
			{
			if (obj == null || !(obj is ServiceClassModel))
				return false;
			var cl1 = ((ServiceClassModel) obj).ServiceClassName.ToLowerInvariant();
			var cl2 = this.ServiceClassName.ToLowerInvariant();
			return cl1==cl2;
			}
		}
	}
