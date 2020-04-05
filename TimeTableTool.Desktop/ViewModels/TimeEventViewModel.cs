using Caliburn.Micro;
using DataAccess.Library.Logic;
using DataAccess.Library.Models;
using System.Linq;
using TimeTableTool.Desktop.Models;

namespace TimeTableTool.Desktop.ViewModels
  {
  public class TimeEventViewModel: Screen
    {
    public TimeEventUIModel TimeEvents { get; set; } = new TimeEventUIModel();
    public int RouteId { get; set; } = -1;
    public int ServiceId { get; set; } = -1;

    public TimeEventViewModel()
      {
      TimeEvents.LocationList= new BindableCollection<LocationModel>(LocationDataAccess.GetAllLocationsPerRoute(RouteId));
      var temp = FullTimeEventDataAccess.GetAllFullTimeEventsPerService(ServiceId)
                              .OrderBy(p => p.Order)
                              .ToList();
      TimeEvents.FilteredFullTimeEventList= new BindableCollection<FullTimeEventModel>(temp);
      }

    protected override async void OnViewLoaded(object view)
      {
      base.OnViewLoaded(view);
      TimeEvents.LocationList= new BindableCollection<LocationModel>(LocationDataAccess.GetAllLocationsPerRoute(RouteId));
      var temp = FullTimeEventDataAccess.GetAllFullTimeEventsPerService(ServiceId)
        .OrderBy(p => p.Order)
        .ToList();
      TimeEvents.FilteredFullTimeEventList= new BindableCollection<FullTimeEventModel>(temp);

      NotifyOfPropertyChange(()=>TimeEvents);
      }

    }
  }
