using Caliburn.Micro;
using DataAccess.Library.Logic;
using DataAccess.Library.Models;
using Logging.Library;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TimetableTool.Desktop.EventModels;
using TimetableTool.Desktop.Helpers;
using TimetableTool.Desktop.Models;

namespace TimetableTool.Desktop.ViewModels
  {
  public class RouteViewModel : Screen
    {
    private readonly IEventAggregator _events;

    private RouteModel _selectedRoute;
    private string _routeDescription;
    private string _routeName;
    private string _routeAbbrev;

    public BindableCollection<RouteModel> RouteList { get; set; }
    public string RouteDescription
      {
      get
        {
        return _routeDescription;
        }
      set
        {
        _routeDescription = value;
        NotifyOfPropertyChange(() => RouteDescription);
        NotifyOfPropertyChange(() => CanSaveRoute);
        }
      }
    public string RouteName
      {
      get
        {
        return _routeName;
        }
      set
        {
        _routeName = value;
        NotifyOfPropertyChange(() => RouteName);
        NotifyOfPropertyChange(() => CanSaveRoute);
        }
      }
    public string RouteAbbrev
      {
      get
        {
        return _routeAbbrev;
        }
      set
        {
        _routeAbbrev = value;
        NotifyOfPropertyChange(()=> RouteAbbrev);
        NotifyOfPropertyChange(() => CanSaveRoute);
        }
      }
    public int RouteId { get; set; }

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
          NotifyOfPropertyChange(() => CanEditRoute);
          NotifyOfPropertyChange(() => CanDeleteRoute);
          NotifyOfPropertyChange(() => CanExportRoute);
          }
        }
      }

    public RouteViewModel(IEventAggregator events)
      {
      _events = events;
      }

    protected override void OnViewLoaded(object view)
      {
      base.OnViewLoaded(view);
      RouteList = new BindableCollection<RouteModel>(RouteDataAccess.GetAllRoutes());
      NotifyOfPropertyChange(() => RouteList);
      }

    public bool CanEditRoute
      {
      // Avoid to to interfere 
      get { return SelectedRoute != null && RouteId == 0; }
      }

    public bool CanDeleteRoute
      {
      get { return SelectedRoute!=null  && Settings.DatabaseVersion>=2; }
      }

    public bool CanExportRoute
      {
      // Avoid to to interfere 
      get { return SelectedRoute != null && RouteId == 0; }
      }

    public bool CanSaveRoute
      {
      get
        {
        return RouteName?.Length > 0
               && RouteAbbrev?.Length > 0
               && RouteDescription?.Length > 0;
        }
      }

    public void EditRoute()
      {
      RouteName = SelectedRoute.RouteName;
      RouteAbbrev = SelectedRoute.RouteAbbreviation;
      RouteDescription = SelectedRoute.RouteDescription;
      RouteId = SelectedRoute.Id;
      NotifyOfPropertyChange(() => CanEditRoute);
      NotifyOfPropertyChange(() => CanDeleteRoute);
      }

    public void DeleteRoute()
      {

      RouteDataAccess.DeleteRoute(SelectedRoute.Id);
      RouteList.Remove(SelectedRoute);
      RouteId = 0;
      NotifyOfPropertyChange(() => RouteList);
      }

    public void SaveRoute()
      {
      RouteModel newRoute = new RouteModel();
      newRoute.RouteName = RouteName;
      newRoute.RouteAbbreviation = RouteAbbrev;
      newRoute.RouteDescription = RouteDescription;
      if (RouteId == 0)
        {
        RouteDataAccess.InsertRoute(newRoute);
        }
      else
        {
        newRoute.Id = RouteId;
        RouteDataAccess.UpdateRoute(newRoute);
        }

      ClearRoute();
      RouteList = new BindableCollection<RouteModel>(RouteDataAccess.GetAllRoutes());
      NotifyOfPropertyChange(() => RouteList);
      }

    public void ClearRoute()
      {
      RouteName = "";
      RouteAbbrev = "";
      RouteDescription = "";
      RouteId = 0;
      NotifyOfPropertyChange(() => CanEditRoute);
      NotifyOfPropertyChange(() => CanDeleteRoute);
      NotifyOfPropertyChange(() => CanExportRoute);
      }


    public void ExportRoute()
      {
      DataAccess.Library.Logic.ExportRouteDataAccess exportRoute= new DataAccess.Library.Logic.ExportRouteDataAccess(SelectedRoute.Id);
      var output = "";
      output += exportRoute.ExportRouteTable();
      output += exportRoute.ExportLocationsTable();
      output += exportRoute.ExportServiceDirectionTable();
      output += exportRoute.ExportServiceTemplateTable();
      output += exportRoute.ExportTimeEventsTable();
      output += exportRoute.ExportServiceTable();
      output += exportRoute.ExportTimetableTable();
      output += exportRoute.ExportConnectTiSi();
      output += exportRoute.ExportTrains();
      output += exportRoute.ExportTrainServices();

      var path= $"{Settings.DataPath}{SelectedRoute.RouteAbbreviation}-{DateTime.UtcNow.ToShortDateString()}.ttt";

      File.WriteAllText(path,output);
      Log.Trace($"Exported route to {path}", LogEventType.Event);
      }


    public void ImportRoute()
      {
      var openFile= new OpenFileModel();
      openFile.CheckFileExists = true;
      openFile.InitialDirectory = Settings.DataPath;
      openFile.Filter="ttt files|*.ttt|All Files|*.*";
      openFile.Title = "Open exported database file";
      var path = FileIOHelper.GetOpenFileName(openFile);
      if (path.Length > 0)
        {
        var importRoute = new ImportRouteDataAccess(path);
        RouteList = new BindableCollection<RouteModel>(RouteDataAccess.GetAllRoutes());
        NotifyOfPropertyChange(() => RouteList);
        }
      }
    }
  }

