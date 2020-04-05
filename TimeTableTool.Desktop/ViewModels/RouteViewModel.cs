using Caliburn.Micro;
using DataAccess.Library.Logic;
using DataAccess.Library.Models;
using TimeTableTool.Desktop.EventModels;
using TimeTableTool.Desktop.Models;

namespace TimeTableTool.Desktop.ViewModels
  {
  public class RouteViewModel: Screen
    {
    private readonly IEventAggregator _events;

    public RouteUIModel RoutesUI { get; set; }
    private RouteModel _selectedRoute;
    public BindableCollection<RouteModel> RouteList { get; set; }
    public RouteModel SelectedRoute
      {
      get { return _selectedRoute; }
      set
        {
        if (_selectedRoute != value)
          {
          _selectedRoute = value;
          RouteSelectedEvent routeSelectedEvent = new RouteSelectedEvent();
          routeSelectedEvent.SelectedRoute = _selectedRoute;
          _events.PublishOnUIThreadAsync(routeSelectedEvent);
          NotifyOfPropertyChange(() => SelectedRoute);
          }
        }
      }

    public RouteViewModel(IEventAggregator events)
      {
      _events = events;
      RoutesUI = new RouteUIModel();
      RouteList = new BindableCollection<RouteModel>(RouteDataAccess.GetAllRoutes());
      } 
    }
  }
