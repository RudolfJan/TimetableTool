using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Library.Models
  {
  public class TimetableModel
    {
    public int Id { get; set; }
    public string TimetableName { get; set; }
    public string TimetableAbbreviation { get; set; }
    public string TimetableDescription { get; set; }
    public bool IsMultiDirection  {get ;set; }
    public int ServiceDirectionId { get; set; }
    public int RouteId { get; set; }
    }
  }
