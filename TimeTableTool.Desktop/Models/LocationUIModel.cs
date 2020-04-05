using DataAccess.Library.Models;
using System.ComponentModel;

namespace TimeTableTool.Desktop.Models
  {
  public class LocationUIModel
    {
    public BindingList<LocationModel> LocationList { get; set; }
    public LocationModel LocationDetails { get; set; }= new LocationModel();
    public LocationFilterModel Filter { get; set; } = new LocationFilterModel();
    public string RouteName { get; set; }
    }
  }
