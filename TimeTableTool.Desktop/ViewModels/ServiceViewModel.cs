using Caliburn.Micro;
using DataAccess.Library.Logic;
using DataAccess.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TimeTableTool.Desktop.EventModels;
using TimeTableTool.Desktop.Models;

namespace TimeTableTool.Desktop.ViewModels
  {
  public class ServiceViewModel: Screen
    {
    private readonly IEventAggregator _events;
    public ServiceUIModel  ServicesUI { get; set; }= new ServiceUIModel();
    public int RouteId { get; set; } = -1;

    private ServiceModel _selectedService;

    public ServiceModel SelectedService
      {
      get { return _selectedService; }
      set
        {
        _selectedService = value;
        ServiceSelectedEvent serviceSelectedEvent=new ServiceSelectedEvent();
        serviceSelectedEvent.SelectedService = _selectedService;
        _events.PublishOnUIThreadAsync(serviceSelectedEvent);
        NotifyOfPropertyChange(() => SelectedService);
        }
      }

 

    public ServiceViewModel(IEventAggregator events)
      {
      _events = events;
      }

    protected override async void OnViewLoaded(object view)
      {
      base.OnViewLoaded(view);
      RouteModel rm = RouteDataAccess.GetRouteById(RouteId);
      ServicesUI.RouteName = rm.RouteName;
      RouteId = rm.Id;
      ServicesUI.ServiceList = new BindingList<ServiceModel>(ServiceDataAccess.GetServicesPerRoute(RouteId));
      NotifyOfPropertyChange(()=>ServicesUI);
      }
    }
  }
