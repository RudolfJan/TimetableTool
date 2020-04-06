using Caliburn.Micro;
using DataAccess.Library.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using TimeTableTool.Desktop.EventModels;

namespace TimeTableTool.Desktop.ViewModels
  {
  public class ShellViewModel : Conductor<object>, IHandle<RouteSelectedEvent>, IHandle<ServiceSelectedEvent>
    //, IHandle<LocationSelectedEvent>, IHandle<ServiceSelectedEvent>
    {
    private readonly IEventAggregator _events;
    private readonly IWindowManager _windowManager;

    public bool IsEditLocationsEnabled
      {
      get { return SelectedRoute != null; }
      }

    public bool IsEditTimeTablesEnabled
      {
      get { return SelectedRoute != null; }
      }

    public bool IsEditServicesEnabled
      {
      get { return SelectedRoute != null; }
      }

    public bool IsEditTimeEventsEnabled
      {
      get
        {
        return SelectedRoute != null && SelectedService!=null;
        }
      }

  private RouteModel _selectedRoute;
    public RouteModel SelectedRoute
      {
      get
        {
        return _selectedRoute;
        }
      set
        {
        _selectedRoute = value;
        NotifyOfPropertyChange(()=>IsEditLocationsEnabled);
        NotifyOfPropertyChange(()=>IsEditTimeTablesEnabled);
        NotifyOfPropertyChange(()=> IsEditServicesEnabled);
        NotifyOfPropertyChange(()=> IsEditTimeEventsEnabled);
        }
      }

    private ServiceModel _selectedService;
    public ServiceModel SelectedService
      {
      get { return _selectedService; }
      set
        {
        _selectedService = value;
        NotifyOfPropertyChange(()=> IsEditTimeEventsEnabled);
        }
      }

    public ShellViewModel(IEventAggregator events, IWindowManager windowManager)
      {
      _events = events;
      _windowManager = windowManager;
      _events.SubscribeOnPublishedThread(this);
      }

    protected override async void OnViewLoaded(object view)
      {
      base.OnViewLoaded(view);
      await ActivateItemAsync(IoC.Get<RouteViewModel>(), new CancellationToken());
      }
    void Location()
      {
      ActivateItemAsync(IoC.Get<LocationViewModel>(), new CancellationToken());
      }

    public async Task ExitApplication()
      {
      await TryCloseAsync();
      }

    public async Task EditLocations()
      {
      var locationVM = IoC.Get<LocationViewModel>();
      locationVM.RouteId = SelectedRoute.Id;
      await ActivateItemAsync(locationVM, new CancellationToken());
      }

    public async Task EditTimeTables()
      {
      var timeTableVM = IoC.Get<TimeTableViewModel>();
      timeTableVM.RouteId = SelectedRoute.Id;
      await ActivateItemAsync(timeTableVM, new CancellationToken());
      }

    public async Task EditServices()
      {
      var servicesVM = IoC.Get<ServiceViewModel>();
      servicesVM.RouteId = SelectedRoute.Id;
      await ActivateItemAsync(servicesVM, new CancellationToken());
      }

    public async Task EditTimeEvents()
      {
      var timeEventsVM = IoC.Get<TimeEventViewModel>();
      timeEventsVM.RouteId = SelectedRoute.Id;
      timeEventsVM.ServiceId = SelectedService.Id;
      await ActivateItemAsync(timeEventsVM, new CancellationToken());
      }


    public async Task ShowAbout()
      {
      await _windowManager.ShowDialogAsync(IoC.Get<AboutViewModel>());
      }


    #region event handlers
 
    public Task HandleAsync(ServiceSelectedEvent message, CancellationToken cancellationToken)
      {
      SelectedService = message.SelectedService;
      return Task.CompletedTask;
      }
  
    #endregion

    /// <summary>Handles the message.</summary>
    /// <param name="message">The message.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A task that represents the asynchronous coroutine.</returns>
    public Task HandleAsync(RouteSelectedEvent message, CancellationToken cancellationToken)
      {
      SelectedRoute = message.SelectedRoute;
      return Task.CompletedTask;
      }
    }
  }
