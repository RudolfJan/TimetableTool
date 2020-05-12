using System.Collections.Generic;

namespace DataAccess.Library.Models
	{
	public class TimetableMatrixModel
		{
		public string TimetableName { get; set; }
		public int	TimetableId { get;set;}
		public int RouteId	{get;set; }
		public bool IsDescending {get;set;} 
		public List<List<ServiceTimingModel>> TimingList { get;set; } = new List<List<ServiceTimingModel>>();
		public string[][] Matrix { get; set; }
		}
	}
