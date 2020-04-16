using Caliburn.Micro;
using DataAccess.Library.Logic;
using DataAccess.Library.Models;
using System.ComponentModel;
using TimetableTool.Desktop.Models;

namespace TimetableTool.Desktop.ViewModels
  {
  public class LocationViewModel : Screen
    {
    #region Properties
    private System.String _locationName;
    private System.String _locationAbbreviation;
    private System.Int32 _order;
    private System.Int32 _numberOfTracks;
    private LocationModel _selectedLocation;

    public LocationUIModel LocationsUI { get; set; } = new LocationUIModel();
    public int RouteId { get; set; } = -1;

    public int LocationId { get; set; } = -1;
    public string LocationName
      {
      get
        {
        return _locationName;
        }
      set
        {
        _locationName = value;
        NotifyOfPropertyChange(() => LocationName);
        NotifyOfPropertyChange(()=> CanSaveLocation);
        }
      }

    public string LocationAbbreviation
      {
      get
        {
        return _locationAbbreviation;
        }
      set
        {
        _locationAbbreviation = value;
        NotifyOfPropertyChange(() => LocationAbbreviation);
        NotifyOfPropertyChange(()=> CanSaveLocation);
        }
      }

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
        NotifyOfPropertyChange(()=> CanEditLocation);
        NotifyOfPropertyChange(()=> CanDeleteLocation);
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
        NotifyOfPropertyChange(()=> CanSaveLocation);
        }
      }

    public int NumberOfTracks
      {
      get
        {
        return _numberOfTracks;
        }
      set
        {
        _numberOfTracks = value;
        NotifyOfPropertyChange(() => NumberOfTracks);
        NotifyOfPropertyChange(()=> CanSaveLocation);
        }
      }

    #endregion

    #region Initialize
    protected override async void OnViewLoaded(object view)
      {
      base.OnViewLoaded(view);
      RouteModel rm = RouteDataAccess.GetRouteById(RouteId);
      LocationsUI.RouteName = rm.RouteName;
      RouteId = rm.Id;
      LocationsUI.LocationList = new BindableCollection<LocationModel>(LocationDataAccess.GetAllLocationsPerRoute(RouteId));
      NotifyOfPropertyChange(() => LocationsUI);
      }
    #endregion

    #region Commands

    public bool CanEditLocation
      {
      get { return SelectedLocation != null && LocationId<=0; }
      }

    public void EditLocation()
      {
      LocationName = SelectedLocation.LocationName;
      LocationAbbreviation = SelectedLocation.LocationAbbreviation;
      Order = SelectedLocation.Order;
      NumberOfTracks = SelectedLocation.NumberOfTracks;
      LocationId = SelectedLocation.Id;
      RouteId = SelectedLocation.RouteId;
      NotifyOfPropertyChange(()=> CanEditLocation);
      }

    public bool CanDeleteLocation
      {
      get
        {
        return false;

        }
      }

    public void DeleteLocation()
      {
      // TODO implement this
      }

    public bool CanSaveLocation
      {
      get
        {
        return LocationName?.Length > 0
               && LocationAbbreviation?.Length > 0
               && Order > 0
               && NumberOfTracks >0;
        }
      }

    public void SaveLocation()
      {
      var newLocation = new LocationModel();
      newLocation.LocationName = LocationName;
      newLocation.LocationAbbreviation = LocationAbbreviation;
      newLocation.Order = Order;
      newLocation.NumberOfTracks = NumberOfTracks;
      newLocation.RouteId = RouteId;
      if (LocationId <= 0)
        {
        LocationDataAccess.InsertLocationForRoute(newLocation);
        }
      else
        {
        newLocation.Id = LocationId;
        LocationDataAccess.UpdateLocationForRoute(newLocation);
        }
      LocationsUI.LocationList = new BindableCollection<LocationModel>(LocationDataAccess.GetAllLocationsPerRoute(RouteId));
      NotifyOfPropertyChange(() => LocationsUI.LocationList);
      NotifyOfPropertyChange(() => LocationsUI);
      ClearLocation();
      }

    public void ClearLocation()
      {
      LocationName = "";
      LocationAbbreviation = "";
      Order = 0;
      NumberOfTracks = 0;
      LocationId = 0;
      NotifyOfPropertyChange(()=> CanEditLocation);
      NotifyOfPropertyChange(()=> CanSaveLocation);
      }

    #endregion


    }
  }
