using DataAccess.Library.Models;
using System.ComponentModel;

namespace TimetableTool.Desktop.Models
  {
  public class LocationUIModel
    {
    public BindingList<LocationModel> LocationList { get; set; }
    public LocationFilterModel Filter { get; set; } = new LocationFilterModel();
    public string RouteName { get; set; }
    }
  }
