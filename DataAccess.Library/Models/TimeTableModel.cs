using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Library.Models
  {
  public class TimeTableModel
    {
    public int Id { get; set; }
    public string TimeTableName { get; set; }
    public string TimeTableAbbreviation { get; set; }
    public string TimeTableDescription { get; set; }
    public int RouteId { get; set; }
    }
  }
