using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Library.Models
  {
  public class ServiceInstanceModel
    {
    public int Id { get; set; }
    public string ServiceInstanceName { get; set; }
    public string ServiceInstanceAbbreviation { get; set; }
    public int StartTime { get; set; }
    public int EndTime { get; set; }
    public int ServiceId { get; set; }
    public int TimetableId { get; set; }
    }
  }
