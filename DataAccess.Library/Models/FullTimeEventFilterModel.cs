using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Library.Models
  {
  public class FullTimeEventFilterModel
    {
    public string EventType { get; set; }
    public int Order { get; set; }
    public string LocationName { get; set; }
    public string LocationAbbreviation { get; set; }
    }
  }
