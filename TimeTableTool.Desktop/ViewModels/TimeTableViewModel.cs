using DataAccess.Library.Logic;
using DataAccess.Library.Models;
using System.ComponentModel;
using TimetableTool.Desktop.Models;
using Caliburn.Micro;
using TimetableTool.Desktop.EventModels;

namespace TimetableTool.Desktop.ViewModels
  {
  public class TimetableViewModel : Screen
    {
    private System.String _TimetableName;
    private System.String _TimetableAbbreviation;
    private TimetableModel _selectedTimetable;
    private System.String _timetableDescription;
    private readonly IEventAggregator _events;

    #region Properties
    public TimetableUIModel TimetablesUI { get; set; } = new TimetableUIModel();
    public int RouteId { get; set; } = -1;
    public int TimetableId { get; set; } = -1;

 
    public string TimetableName
      {
      get
        {
        return _TimetableName;
        }
      set
        {
        _TimetableName = value;
        NotifyOfPropertyChange(() => TimetableName);
        NotifyOfPropertyChange(() => CanSaveTimetable);
        }
      }

    public string TimetableAbbreviation
      {
      get
        {
        return _TimetableAbbreviation;
        }
      set
        {
        _TimetableAbbreviation = value;
        NotifyOfPropertyChange(() => TimetableAbbreviation);
        NotifyOfPropertyChange(() => CanSaveTimetable);
        }
      }

    public string TimetableDescription
      {
      get
        {
        return _timetableDescription;
        }
      set
        {
        _timetableDescription = value;
        NotifyOfPropertyChange(() => TimetableDescription);
        NotifyOfPropertyChange(() => CanSaveTimetable);
        }
      }

    public TimetableModel SelectedTimetable
      {
      get
        {
        return _selectedTimetable;
        }
      set
        {
        _selectedTimetable = value;
        TimetableSelectedEvent timetableSelectedEvent = new TimetableSelectedEvent();
        timetableSelectedEvent.SelectedTimetable = _selectedTimetable;
        _events.PublishOnUIThreadAsync(timetableSelectedEvent);
        NotifyOfPropertyChange(() => SelectedTimetable);
        NotifyOfPropertyChange(() => CanEditTimetable);
        NotifyOfPropertyChange(() => CanDeleteTimetable);
        }
      }

    #endregion

    #region Initialization

    public TimetableViewModel(IEventAggregator events)
      {
      _events = events;
      }
    protected override async void OnViewLoaded(object view)
      {
      base.OnViewLoaded(view);
      RouteModel rm = RouteDataAccess.GetRouteById(RouteId);
      TimetablesUI.RouteName = rm.RouteName;
      RouteId = rm.Id;
      TimetablesUI.TimetableList = new BindingList<TimetableModel>(TimetableDataAccess.GetAllTimetablesPerRoute(RouteId));
      NotifyOfPropertyChange(() => TimetablesUI);
      }
    #endregion

    public bool CanEditTimetable
      {
      get { return SelectedTimetable != null && TimetableId <= 0; }
      }

    public void EditTimetable()
      {
      TimetableName = SelectedTimetable.TimetableName;
      TimetableAbbreviation = SelectedTimetable.TimetableAbbreviation;
      TimetableDescription = SelectedTimetable.TimetableDescription;
      TimetableId = SelectedTimetable.Id;
      }

    public bool CanDeleteTimetable
      {
      get { return false; }
      }

    public bool CanSaveTimetable
      {
      get
        {
        return TimetableDescription?.Length>0
               && TimetableName?.Length>0
               && TimetableAbbreviation?.Length > 0;
        }
      }

    public void SaveTimetable()
      {
      var newTimetable = new TimetableModel();
      newTimetable.TimetableDescription = TimetableDescription;
      newTimetable.TimetableName = TimetableName;
      newTimetable.TimetableAbbreviation = TimetableAbbreviation;
      newTimetable.RouteId = RouteId;
      if (TimetableId <= 0)
        {
        TimetableDataAccess.InsertTimetableForRoute(newTimetable);
        }
      else
        {
        newTimetable.Id = TimetableId;
        TimetableDataAccess.UpdateTimetable(newTimetable);
        }
      ClearTimetable();
      TimetablesUI.TimetableList = new BindingList<TimetableModel>(TimetableDataAccess.GetAllTimetablesPerRoute(RouteId));
      NotifyOfPropertyChange(() => TimetablesUI);
      }

    public void ClearTimetable()
      {
      TimetableDescription = "";
      TimetableAbbreviation = "";
      TimetableName = "";
      TimetableId = 0;
      }
    }
  }
