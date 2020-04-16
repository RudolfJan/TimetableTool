using Caliburn.Micro;
using DataAccess.Library.Models;

namespace TimetableTool.Desktop.Models
  {
  public class LocationUIModel
    {
    public BindableCollection<LocationModel> LocationList { get; set; }
    public LocationFilterModel Filter { get; set; } = new LocationFilterModel();
    public string RouteName { get; set; }
    }
  }
