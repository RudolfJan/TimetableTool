using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Library.Models
  {
  public class FullTimeEventModel
    {
    public int Id { get; set; }
    public string EventType { get; set; }
    public int ArrivalTime { get; set; }
    public int WaitTime { get; set; }
    public int ServiceId { get; set; }
    public int LocationId { get; set; }
    public int Order { get; set; }
    public string LocationName { get; set; }
    public string LocationAbbreviation { get; set; }
    public int NumberOfTracks { get; set; }
    public int LocationOrder { get; set; }
    public string ServiceAbbreviation { get; set; }
    public string ServiceName { get; set; }
    }
  }
