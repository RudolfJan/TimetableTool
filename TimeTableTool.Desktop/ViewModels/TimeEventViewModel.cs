using Caliburn.Micro;
using DataAccess.Library.Logic;
using DataAccess.Library.Models;
using System.Linq;
using TimetableTool.Desktop.Models;

namespace TimetableTool.Desktop.ViewModels
  {
  public class TimeEventViewModel : Screen
    {
    private System.String _timeType;
    private System.Int32 _time;
    private System.Int32 _order;
    private LocationModel _location;
    private FullTimeEventModel _selectedTimeEvent;
    private LocationModel _selectedLocation;

    #region Properties
    public TimeEventUIModel TimeEvents { get; set; } = new TimeEventUIModel();
    public int RouteId { get; set; } = -1;
    public int ServiceId { get; set; } = -1;
    public int TimeEventId { get; set; } = -1;
    public string ServiceName { get; set; }
    public string RouteName { get; set; }

    public LocationModel SelectedLocation
      {
      get
        {
        return _selectedLocation;
        }
      set
        {
        _selectedLocation = value;
        NotifyOfPropertyChange(()=>SelectedLocation);
        NotifyOfPropertyChange(()=> CanSelectLocation);
        }
      }

    public FullTimeEventModel SelectedTimeEvent
      {
      get
        {
        return _selectedTimeEvent;
        }
      set
        {
        _selectedTimeEvent = value;
        NotifyOfPropertyChange(() => SelectedTimeEvent);
        NotifyOfPropertyChange(() => CanEditTimeEvent);
        }
      }
    public string TimeType
      {
      get
        {
        return _timeType;
        }
      set
        {
        _timeType = value;
        NotifyOfPropertyChange(() => TimeType);
        NotifyOfPropertyChange(() => CanSaveTimeEvent);
        }
      }

    public int Time
      {
      get
        {
        return _time;
        }
      set
        {
        _time = value;
        NotifyOfPropertyChange(() => Time);
        }
      }

    public int Order
      {
      get
        {
        return _order;
        }
      set
        {
        _order = value;
        NotifyOfPropertyChange(() => Order);
        NotifyOfPropertyChange(() => CanSaveTimeEvent);
        }
      }

    public LocationModel Location
      {
      get
        {
        return _location;
        }
      set
        {
        _location = value;
        NotifyOfPropertyChange(() => Location);
        NotifyOfPropertyChange(() => CanSaveTimeEvent);
        }
      }
    #endregion

    #region Initialization
    protected override async void OnViewLoaded(object view)
      {
      base.OnViewLoaded(view);
      TimeEvents.SelectedRoute = RouteDataAccess.GetRouteById(RouteId);
      TimeEvents.SelectedService = ServiceDataAccess.GetServiceById(ServiceId);
      TimeEvents.LocationList = new BindableCollection<LocationModel>(LocationDataAccess.GetAllLocationsPerRoute(RouteId));
      var temp = FullTimeEventDataAccess.GetAllFullTimeEventsPerService(ServiceId)
        .OrderBy(p => p.Order)
        .ToList();
      TimeEvents.FilteredFullTimeEventList = new BindableCollection<FullTimeEventModel>(temp);

      NotifyOfPropertyChange(() => TimeEvents);
      }

    #endregion

    public bool CanEditTimeEvent
      {
      get { return SelectedTimeEvent != null && TimeEventId <= 0; }
      }

    public void EditTimeEvent()
      {
      Location = LocationDataAccess.GetLocationById(SelectedTimeEvent.LocationId);
      TimeType = SelectedTimeEvent.EventType;
      Time = SelectedTimeEvent.RelativeTime;
      Order = SelectedTimeEvent.Order;
      TimeEventId = SelectedTimeEvent.Id;
      NotifyOfPropertyChange(() => CanEditTimeEvent);
      NotifyOfPropertyChange(()=>CanSaveTimeEvent);
      }

    bool CanSelectLocation
      {
      get { return SelectedLocation != null; }
      }

    public bool CanSaveTimeEvent
      {
      get
        {
        return Location != null
               && TimeType?.Length > 0
               && Order > 0;
        }
      }

    public void SaveTimeEvent()
      {
      TimeEventModel newTimeEvent= new TimeEventModel();
      newTimeEvent.EventType = TimeType;
      newTimeEvent.RelativeTime = Time;
      newTimeEvent.LocationId = Location.Id;
      newTimeEvent.Order = Order;
      newTimeEvent.ServiceId = ServiceId;
      if (TimeEventId <= 0)
        {
        TimeEventDataAccess.InsertTimeEventForService(newTimeEvent);
        }
      else
        {
        newTimeEvent.Id = TimeEventId;
        TimeEventDataAccess.UpdateTimeEvent(newTimeEvent);
        }

      var temp = FullTimeEventDataAccess.GetAllFullTimeEventsPerService(ServiceId)
        .OrderBy(p => p.Order)
        .ToList();
      TimeEvents.FilteredFullTimeEventList = new BindableCollection<FullTimeEventModel>(temp);
      UpdateCalculatedDuration();
      NotifyOfPropertyChange(() => TimeEvents);
      ClearTimeEvent();
      }

    public void SelectLocation()
      {
      Location = SelectedLocation;
      NotifyOfPropertyChange(()=>CanSaveTimeEvent);
      }

    public void ClearTimeEvent()
      {
      TimeType = "";
      Order = 0;
      Time = 0;
      TimeEventId = 0;
      Location = null;
      NotifyOfPropertyChange(()=>CanEditTimeEvent);
      NotifyOfPropertyChange(()=>CanSaveTimeEvent);
      NotifyOfPropertyChange(()=>CanSelectLocation);
      }

    private void UpdateCalculatedDuration()
      {
      // TODO do this on the non-filtered list as soon as filtering is implemented
      int duration = TimeEvents.FilteredFullTimeEventList.Sum(x => x.RelativeTime);
      ServiceDataAccess.UpdateServiceCalculatedDuration(duration,ServiceId);
      }
    }
  }
