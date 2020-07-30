using Caliburn.Micro;
using DataAccess.Library.Logic;
using DataAccess.Library.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using TimetableTool.Desktop.Models;

namespace TimetableTool.Desktop.ViewModels
  {
  public class TimeEventViewModel : Screen
    {
    private string _timeType;
    private int _arrivalTime;
    private int _waitTime;
    private int _order;
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
        NotifyOfPropertyChange(() => CanDeleteTimeEvent);
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

    public int ArrivalTime
      {
      get
        {
        return _arrivalTime;
        }
      set
        {
        _arrivalTime = value;
        NotifyOfPropertyChange(() => ArrivalTime);
        }
      }

       public int WaitTime
      {
      get
        {
        return _waitTime;
        }
      set
        {
        _waitTime = value;
        NotifyOfPropertyChange(() => WaitTime);
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

		private List<string> _timeEventTypeList;

		public List<string> TimeEventTypeList  
			{
			get { return _timeEventTypeList; }
			set { _timeEventTypeList = value; }
			}

    private string _selectedTimeEventType;

    public string SelectedTimeEventType
      {
      get
        {
        return _selectedTimeEventType;
        }
      set
        {
        _selectedTimeEventType = value;
        NotifyOfPropertyChange(()=>SelectedTimeEventType);
        }
      }

    #endregion

    #region Initialization
    protected override async void OnViewLoaded(object view)
      {
      base.OnViewLoaded(view);
      TimeEvents.SelectedRoute = RouteDataAccess.GetRouteById(RouteId);
      TimeEvents.SelectedService = ServiceTemplateDataAccess.GetServiceTemplateById(ServiceId);
      TimeEvents.LocationList = new BindableCollection<LocationModel>(LocationDataAccess.GetAllLocationsPerRoute(RouteId));
      var temp = FullTimeEventDataAccess.GetAllFullTimeEventsPerServiceTemplate(ServiceId)
        .OrderBy(p => p.Order)
        .ToList();
      TimeEvents.FilteredFullTimeEventList = new BindableCollection<FullTimeEventModel>(temp);
      TimeEventTypeList = TimeEventTypeDataAccess.GetAllTimeEventTypeStrings();
      NotifyOfPropertyChange(()=>TimeEventTypeList);
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
      ArrivalTime = SelectedTimeEvent.ArrivalTime;
      WaitTime=SelectedTimeEvent.WaitTime;
      Order = SelectedTimeEvent.Order;
      TimeEventId = SelectedTimeEvent.Id;
      NotifyOfPropertyChange(() => CanEditTimeEvent);
      NotifyOfPropertyChange(()=>CanSaveTimeEvent);
      NotifyOfPropertyChange(()=> TimeEventTypeList);
      NotifyOfPropertyChange(() => TimeType);
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
               && TimeType!=null
               && Order > 0;
        }
      }

    public void SaveTimeEvent()
      {
      TimeEventModel newTimeEvent= new TimeEventModel();
      newTimeEvent.EventType = TimeType;
      newTimeEvent.ArrivalTime = ArrivalTime;
      newTimeEvent.WaitTime = WaitTime;
      newTimeEvent.LocationId = Location.Id;
      newTimeEvent.Order = Order;
      newTimeEvent.ServiceTemplateId = ServiceId;
      if (TimeEventId <= 0)
        {
        TimeEventDataAccess.InsertTimeEventForServiceTemplate(newTimeEvent);
        }
      else
        {
        newTimeEvent.Id = TimeEventId;
        TimeEventDataAccess.UpdateTimeEvent(newTimeEvent);
        }

      var temp = FullTimeEventDataAccess.GetAllFullTimeEventsPerServiceTemplate(ServiceId)
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
      SelectedTimeEvent = null;
      Order = 0;
      ArrivalTime = 0;
      WaitTime=0;
      TimeEventId = 0;
      Location = null;
      NotifyOfPropertyChange(()=>CanEditTimeEvent);
      NotifyOfPropertyChange(()=>CanSaveTimeEvent);
      NotifyOfPropertyChange(()=>CanSelectLocation);
      NotifyOfPropertyChange(()=> TimeEventTypeList);
      }

    private void UpdateCalculatedDuration()
      {
      // TODO do this on the non-filtered list as soon as filtering is implemented
      int duration = TimeEvents.FilteredFullTimeEventList.Sum(x => x.ArrivalTime+x.WaitTime);
      ServiceTemplateDataAccess.UpdateServiceCalculatedDuration(duration,ServiceId);
      }

    public bool CanDeleteTimeEvent
      {
      get { return SelectedTimeEvent != null  && Settings.DatabaseVersion>=2; }
      }

		public void DeleteTimeEvent()
      {
      TimeEventDataAccess.DeleteTimeEvent(SelectedTimeEvent.Id);
      TimeEvents.FilteredFullTimeEventList.Remove(SelectedTimeEvent);
      TimeEventId = 0;
      }
    }
  }
