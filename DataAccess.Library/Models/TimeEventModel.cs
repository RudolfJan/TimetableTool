namespace DataAccess.Library.Models
  {
  public class TimeEventModel
    {
    public int Id { get; set; }
    public string EventType { get; set; }
    public int ArrivalTime { get; set; }
    public int WaitTime { get; set; }
    public int ServiceId { get; set; }
    public int LocationId { get; set; }
    public int Order { get; set; }
    }
  }
