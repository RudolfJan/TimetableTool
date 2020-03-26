using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Library.Models
  {
  public class LocationModel
    {
    public int Id { get; set; }
    public string LocationName { get; set; }
    public string LocationAbbreviation { get; set; }
    public int NumberOfTracks { get; set; }
    public int Order { get; set; }
    public int RouteId { get; set; }
    }
  }
