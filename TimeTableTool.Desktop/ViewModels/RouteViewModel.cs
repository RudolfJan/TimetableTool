using Caliburn.Micro;
using DataAccess.Library.Logic;
using DataAccess.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TimeTableTool.Desktop.Models;

namespace TimeTableTool.Desktop.ViewModels
  {
  public class RouteViewModel: Screen
    {
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
          //GetScenarios(_selectedRoute)
          NotifyOfPropertyChange(() => SelectedRoute);
          }
        }
      }

    public RouteViewModel()
      {
      RoutesUI = new RouteUIModel();
      RouteList = new BindableCollection<RouteModel>(RouteDataAccess.GetAllRoutes());
      } 
    }
  }
