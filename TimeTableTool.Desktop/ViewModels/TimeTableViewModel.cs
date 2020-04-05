using Caliburn.Micro;
using DataAccess.Library.Logic;
using DataAccess.Library.Models;
using System.ComponentModel;
using TimeTableTool.Desktop.Models;

namespace TimeTableTool.Desktop.ViewModels
  {
  public class TimeTableViewModel: Screen
    {
    public TimeTableUIModel TimeTablesUI { get; set; } = new TimeTableUIModel();
    public int RouteId { get; set; } = -1;
    public TimeTableViewModel()
      {
      }

    protected override async void OnViewLoaded(object view)
      {
      base.OnViewLoaded(view);
      RouteModel rm = RouteDataAccess.GetRouteById(RouteId);
      TimeTablesUI.RouteName = rm.RouteName;
      RouteId = rm.Id;
      TimeTablesUI.TimeTableList = new BindingList<TimeTableModel>(TimeTableDataAccess.GetAllTimeTablesPerRoute(RouteId));
      NotifyOfPropertyChange(()=>TimeTablesUI);
      }
    }


  }
