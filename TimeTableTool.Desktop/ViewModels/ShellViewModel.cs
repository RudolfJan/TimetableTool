﻿using Caliburn.Micro;
using DataAccess.Library.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using TimetableTool.Desktop.EventModels;

namespace TimetableTool.Desktop.ViewModels
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

    public bool IsEditTimetablesEnabled
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
        NotifyOfPropertyChange(()=>IsEditTimetablesEnabled);
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
      await EditRoutes();
      }
    void Location()
      {
      ActivateItemAsync(IoC.Get<LocationViewModel>(), new CancellationToken());
      }

    public async Task ExitApplication()
      {
      await TryCloseAsync();
      }

    public async Task EditRoutes()
      {
      var routeVM = IoC.Get<RouteViewModel>();
      await ActivateItemAsync(routeVM, new CancellationToken());
      }

    public async Task EditLocations()
      {
      var locationVM = IoC.Get<LocationViewModel>();
      locationVM.RouteId = SelectedRoute.Id;
      await ActivateItemAsync(locationVM, new CancellationToken());
      }

    public async Task EditTimetables()
      {
      var timetableVM = IoC.Get<TimetableViewModel>();
      timetableVM.RouteId = SelectedRoute.Id;
      await ActivateItemAsync(timetableVM, new CancellationToken());
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
  
    public Task HandleAsync(RouteSelectedEvent message, CancellationToken cancellationToken)
      {
      // If you change the selected route, you must reselect the service, because it is attached to the route.
      if (SelectedRoute?.Id != message.SelectedRoute.Id)
        {
        SelectedService = null;
        NotifyOfPropertyChange(()=> IsEditTimeEventsEnabled);
        }
      SelectedRoute = message.SelectedRoute;
      return Task.CompletedTask;
      }
    #endregion
    }
  }
