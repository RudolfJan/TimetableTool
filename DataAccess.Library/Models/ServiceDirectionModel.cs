namespace DataAccess.Library.Models
	{
	public class ServiceDirectionModel
		{
		public int Id { get; set; }
		public string ServiceDirectionName { get; set; }
		public string ServiceDirectionAbbreviation { get; set; }
		public int RouteId { get; set; }
		public bool IsDescending { get; set; }
		}
	}
