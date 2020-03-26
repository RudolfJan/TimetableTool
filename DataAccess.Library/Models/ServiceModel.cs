using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Library.Models
  {
  public class ServiceModel
    {
    public int Id { get; set; }
    public string ServiceName { get; set; }
    public string ServiceAbbreviation { get; set; }
    public string ServiceType { get; set; }
    public int CalculatedDuration { get; set; }
    public int RouteId { get; set; }
    }
  }
