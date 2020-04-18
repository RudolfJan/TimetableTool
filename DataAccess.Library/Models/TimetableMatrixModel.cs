using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace DataAccess.Library.Models
	{
	public class TimetableMatrixModel
		{
		public string TimetableName { get; set; }
		public int	TimetableId { get;set;}
		public int RouteId	{get;set; }
		public bool IsDescending {get;set;} 
		public List<List<ServiceInstanceTimingModel>> TimingList { get;set; } = new List<List<ServiceInstanceTimingModel>>();
		public string[][] Matrix { get; set; }
		}
	}
