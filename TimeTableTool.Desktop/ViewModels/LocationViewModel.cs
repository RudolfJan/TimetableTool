using Caliburn.Micro;
using DataAccess.Library.Logic;
using DataAccess.Library.Models;
using System.ComponentModel;
using TimeTableTool.Desktop.Models;

namespace TimeTableTool.Desktop.ViewModels
  {
  public class LocationViewModel: Screen
    {
    public LocationUIModel LocationsUI { get; set; } = new LocationUIModel();
    public int RouteId { get; set; } = -1;

    public LocationViewModel()
      {

      }

    protected override async void OnViewLoaded(object view)
      {
      base.OnViewLoaded(view);
      RouteModel rm = RouteDataAccess.GetRouteById(RouteId);
      LocationsUI.RouteName = rm.RouteName;
      RouteId = rm.Id;
      LocationsUI.LocationList = new BindingList<LocationModel>(LocationDataAccess.GetAllLocationsPerRoute(RouteId));
      NotifyOfPropertyChange(()=>LocationsUI);
      }
    }
  }
