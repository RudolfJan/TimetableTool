using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Library.Models
  {
  public class TimeEventModel
    {
    public int Id { get; set; }
    public string EventType { get; set; }
    public int RelativeTime { get; set; }
    public int ServiceId { get; set; }
    public int LocationId { get; set; }
    public int Order { get; set; }
    }
  }
