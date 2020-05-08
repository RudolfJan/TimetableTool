using Caliburn.Micro;
using DataAccess.Library.Logic;
using DataAccess.Library.Models;
using System;
using System.ComponentModel;
using TimetableTool.DataAccessLibrary.Logic;

namespace TimetableTool.Desktop.ViewModels
  {
  public class ServiceInstanceViewModel : Screen
    {
    #region Properties

    private string _startTimeText;

    private string _endTimeText;
    private ServiceInstanceModel _selectedServiceInstance;
    private string _serviceInstanceName;
    private string _serviceInstanceAbbreviation;
    private ServiceModel _service;
    private ServiceModel _selectedService;

    public int RouteId { get; set; }
    public int ServiceInstanceId { get; set; }
    public string RouteName { get; set; }
    public string TimetableName { get; set; }

    public string ServiceInstanceName
      {
      get
        {
        return _serviceInstanceName;
        }
      set
        {
        _serviceInstanceName = value;
        NotifyOfPropertyChange(()=> ServiceInstanceName);
        NotifyOfPropertyChange(() => CanSaveServiceInstance);
        }
      }

    public string ServiceInstanceAbbreviation
      {
      get
        {
        return _serviceInstanceAbbreviation;
        }
      set
        {
        _serviceInstanceAbbreviation = value;
        NotifyOfPropertyChange(() => ServiceInstanceAbbreviation);
        NotifyOfPropertyChange(() => CanSaveServiceInstance);
        }
      }

    public BindableCollection<ServiceModel> ServiceList { get; set; }
    public BindableCollection<ServiceInstanceModel> ServiceInstanceList { get; set; }

    public ServiceInstanceModel SelectedServiceInstance
      {
      get
        {
        return _selectedServiceInstance;
        }
      set
        {
        _selectedServiceInstance = value;
        NotifyOfPropertyChange(() => CanEditServiceInstance);
        NotifyOfPropertyChange(() => CanDeleteServiceInstance);
        }
      }


    public ServiceModel SelectedService
      {
      get
        {
        return _selectedService;
        }
      set
        {
        _selectedService = value;
        NotifyOfPropertyChange(()=>CanSelectService);
        }
      }

    public ServiceModel Service
      {
      get
        {
        return _service;
        }
      set
        {
        _service = value;
        NotifyOfPropertyChange(() => Service);
        NotifyOfPropertyChange(() => CanSaveServiceInstance);
        }
      }

    public string StartTimeText
      {
      get { return _startTimeText; }
      set
        {
        _startTimeText = value;
        NotifyOfPropertyChange(() => StartTimeText);
        NotifyOfPropertyChange(() => CanSaveServiceInstance);
        }
      }

    public string EndTimeText
      {
      get { return _endTimeText; }
      set
        {
        _endTimeText = value;
        NotifyOfPropertyChange(() => EndTimeText);
        NotifyOfPropertyChange(() => CanSaveServiceInstance);
        }
      }

    #endregion
    #region Initialize

    protected override async void OnViewLoaded(object view)
      {
      base.OnViewLoaded(view);
      RouteModel rm = RouteDataAccess.GetRouteById(RouteId);
      RouteName = rm.RouteName;
      ServiceList = new BindableCollection<ServiceModel>(ServiceDataAccess.GetServicesPerRoute(RouteId));
      ServiceInstanceList = new BindableCollection<ServiceInstanceModel>(ServiceInstanceDataAccess.GetServiceInstancesPerRoute(RouteId));
      NotifyOfPropertyChange(() => ServiceList);
      NotifyOfPropertyChange(() => ServiceInstanceList);
      NotifyOfPropertyChange(() => RouteName);
      NotifyOfPropertyChange(() => TimetableName);
      }

    #endregion

    public bool CanEditServiceInstance
      {
      get { return SelectedServiceInstance != null && ServiceInstanceId <= 0; }
      }

    public bool CanDeleteServiceInstance
      {
      get { return SelectedServiceInstance != null  && Settings.DatabaseVersion>=2; }
      }

    public void DeleteServiceInstance()
      {
      ServiceInstanceDataAccess.DeleteServiceInstance(SelectedServiceInstance.Id);
      ServiceInstanceList.Remove(SelectedServiceInstance);
      ServiceInstanceId = 0;
      }

    public void SelectService()
      {
      Service = SelectedService;
      }

    public void EditServiceInstance()
      {
      ServiceInstanceName = SelectedServiceInstance.ServiceInstanceName;
      ServiceInstanceAbbreviation = SelectedServiceInstance.ServiceInstanceAbbreviation;
      Service = ServiceDataAccess.GetServiceById(SelectedServiceInstance.ServiceId);
      ServiceInstanceId = SelectedServiceInstance.Id;
      StartTimeText =TimeConverters.MinutesToString(SelectedServiceInstance.StartTime);
      EndTimeText = TimeConverters.MinutesToString(SelectedServiceInstance.EndTime);
      NotifyOfPropertyChange(() => CanEditServiceInstance);
      NotifyOfPropertyChange(() => CanSaveServiceInstance);
      }

    public bool CanSelectService
      {
      get { return SelectedService != null; }
      }

    public bool CanSaveServiceInstance
      {
      get
        {
        return Service != null
               && ServiceInstanceName?.Length > 0
               && ServiceInstanceAbbreviation?.Length > 0;
        }
      }

    public void SaveServiceInstance()
      {
      ServiceInstanceModel newInstance = new ServiceInstanceModel();
      newInstance.ServiceInstanceName = ServiceInstanceName;
      newInstance.ServiceInstanceAbbreviation = ServiceInstanceAbbreviation;
      newInstance.ServiceId = Service.Id;
      newInstance.StartTime = TimeConverters.TimeToMinutes(StartTimeText);
      newInstance.EndTime = TimeConverters.TimeToMinutes(EndTimeText);
      if (ServiceInstanceId <= 0)
        {
        ServiceInstanceDataAccess.InsertServiceInstance(newInstance);
        }
      else
        {
        newInstance.Id = ServiceInstanceId;
        ServiceInstanceDataAccess.UpdateServiceInstance(newInstance);
        }
      ClearServiceInstance();
      ServiceInstanceList = new BindableCollection<ServiceInstanceModel>(ServiceInstanceDataAccess.GetServiceInstancesPerRoute(RouteId));
      NotifyOfPropertyChange(() => ServiceInstanceList);
      }


    public void ClearServiceInstance()
      {
      Service = null;
      StartTimeText = "";
      EndTimeText = "";
      ServiceInstanceName = "";
      ServiceInstanceAbbreviation = "";
      ServiceInstanceId = 0;
      }
    }
  }
