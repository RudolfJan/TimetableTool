using Caliburn.Micro;
using DataAccess.Library.Logic;
using DataAccess.Library.Models;
using Logging.Library;
using System;
using System.Linq;
using System.Windows.Media.Animation;
using TimetableTool.Desktop.EventModels;
using TimetableTool.Desktop.Models;

namespace TimetableTool.Desktop.ViewModels
	{
	public class ServiceViewModel : Screen
		{
		private readonly IEventAggregator _events;
		public ServiceUIModel ServicesUI { get; set; } = new ServiceUIModel();
		public int RouteId { get; set; } = -1;
		public int ServiceId { get; set; } = -1;
		public int ServiceDirectionId { get; set; } = -1;
		private bool _hasTimeEvents;
		public bool HasTimeEvents
			{
			get
				{
				return _hasTimeEvents;
				}
			set
				{
				_hasTimeEvents = value;
				NotifyOfPropertyChange(() => HasTimeEvents);
				}
			}

		private BindableCollection<FullTimeEventModel> _fullTimeEventsList;

		public BindableCollection<FullTimeEventModel> FullTimeEventsList
			{
			get
				{
				return _fullTimeEventsList;
				}
			set
				{
				_fullTimeEventsList = value;
				NotifyOfPropertyChange(() => FullTimeEventsList);
				NotifyOfPropertyChange(() => HasTimeEvents);
				}
			}


		private ServiceModel _selectedService;
		private ServiceDirectionModel _selectedServiceDirection;
		private string _serviceName;
		private string _serviceDescription;
		private string _serviceAbbreviation;
		private string _serviceType;
		private int _calculatedDuration;

		public ServiceModel SelectedService
			{
			get { return _selectedService; }
			set
				{
				_selectedService = value;
				ServiceSelectedEvent serviceSelectedEvent = new ServiceSelectedEvent();
				serviceSelectedEvent.SelectedService = _selectedService;
				_events.PublishOnUIThreadAsync(serviceSelectedEvent);
				if(SelectedService!=null)
					{
					FullTimeEventsList= new BindableCollection<FullTimeEventModel>(FullTimeEventDataAccess.GetAllFullTimeEventsPerService(SelectedService.Id));
					}
				else
					{
					FullTimeEventsList=null;
					}
				NotifyOfPropertyChange(() => CanLoadTimeEvents);
				NotifyOfPropertyChange(() => SelectedService);
				NotifyOfPropertyChange(() => CanEditService);
				NotifyOfPropertyChange(() => CanDeleteService);
				NotifyOfPropertyChange(() => CanLoadTimeEvents);
				}
			}

		public ServiceDirectionModel SelectedServiceDirection
			{
			get { return _selectedServiceDirection; }
			set
				{
				_selectedServiceDirection = value;
				NotifyOfPropertyChange(() => SelectedServiceDirection);
				NotifyOfPropertyChange(() => CanSelectServiceDirection);
				}
			}

		private BindableCollection<ServiceDirectionModel> _serviceDirectionList;
		public BindableCollection<ServiceDirectionModel> ServiceDirectionList
			{
			get
				{
				return _serviceDirectionList;
				}
			set
				{
				_serviceDirectionList = value;
				NotifyOfPropertyChange(() => ServiceDirectionList);
				NotifyOfPropertyChange(() => CanSelectServiceDirection);
				}
			}

		public string ServiceName
			{
			get
				{
				return _serviceName;
				}
			set
				{
				_serviceName = value;
				NotifyOfPropertyChange(() => ServiceName);
				NotifyOfPropertyChange(() => CanSaveService);
				}
			}

		private string _serviceDirectionName;

		public string ServiceDirectionName
			{
			get { return _serviceDirectionName; }
			set
				{
				_serviceDirectionName = value;
				NotifyOfPropertyChange(() => ServiceDirectionName);
				}
			}

		public string ServiceDescription
			{
			get
				{
				return _serviceDescription;
				}
			set
				{
				_serviceDescription = value;
				NotifyOfPropertyChange(() => ServiceDescription);
				NotifyOfPropertyChange(() => CanSaveService);
				}
			}

		public string ServiceAbbreviation
			{
			get
				{
				return _serviceAbbreviation;
				}
			set
				{
				_serviceAbbreviation = value;
				NotifyOfPropertyChange(() => ServiceAbbreviation);
				NotifyOfPropertyChange(() => CanSaveService);
				}
			}

		public string ServiceType
			{
			get
				{
				return _serviceType;
				}
			set
				{
				_serviceType = value;
				NotifyOfPropertyChange(() => ServiceType);
				NotifyOfPropertyChange(() => CanSaveService);
				}
			}

		public int CalculatedDuration
			{
			get
				{
				return _calculatedDuration;
				}
			set
				{
				_calculatedDuration = value;
				NotifyOfPropertyChange(() => CalculatedDuration);
				NotifyOfPropertyChange(() => CanSaveService);
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
			ServicesUI.ServiceList = new BindableCollection<ServiceModel>(ServiceDataAccess.GetServicesPerRoute(RouteId));
			ServiceDirectionList = new BindableCollection<ServiceDirectionModel>(ServiceDirectionDataAccess.GetAllServiceDirectionsPerRoute(RouteId));
			NotifyOfPropertyChange(() => ServicesUI);
			NotifyOfPropertyChange(() => ServiceDirectionList);
			}

		public bool CanEditService
			{
			get { return SelectedService != null && ServiceId <= 0; }
			}


		public void EditService()
			{
			ServiceName = SelectedService.ServiceName;
			ServiceAbbreviation = SelectedService.ServiceAbbreviation;
			ServiceType = SelectedService.ServiceType;
			ServiceDescription = SelectedService.ServiceDescription;
			CalculatedDuration = SelectedService.CalculatedDuration;
			ServiceDirectionId = SelectedService.ServiceDirectionId;
			SelectedServiceDirection = ServiceDirectionDataAccess.GetServiceDirectionById(ServiceDirectionId);
			ServiceDirectionName = SelectedServiceDirection.ServiceDirectionName;
			ServiceId = SelectedService.Id;
			FullTimeEventsList= new BindableCollection<FullTimeEventModel>(FullTimeEventDataAccess.GetAllFullTimeEventsPerService(SelectedService.Id));
			NotifyOfPropertyChange(() => CanLoadTimeEvents);
			NotifyOfPropertyChange(() => CanEditService);
			NotifyOfPropertyChange(() => CanDeleteService);

			}

		public bool CanDeleteService
			{
			get { return false; }
			}
		public bool CanSaveService
			{
			get
				{
				return ServiceAbbreviation?.Length > 0
							 && ServiceName?.Length > 0
							 && ServiceDescription?.Length > 0
							 && ServiceType?.Length > 0
							 && ServiceDirectionId > 0;
				}
			}

		public bool CanSelectServiceDirection
			{
			get
				{
				return SelectedServiceDirection != null;
				}
			}

		public void SelectServiceDirection()
			{
			ServiceDirectionId = SelectedServiceDirection.Id;
			ServiceDirectionName = SelectedServiceDirection.ServiceDirectionName;
			NotifyOfPropertyChange(() => CanSaveService);
			NotifyOfPropertyChange(() => ServiceDirectionName);
			}

		public void SaveService()
			{
			var newService = new ServiceModel();
			newService.CalculatedDuration = CalculatedDuration;
			newService.ServiceAbbreviation = ServiceAbbreviation;
			newService.ServiceDescription = ServiceDescription;
			newService.ServiceName = ServiceName;
			newService.ServiceType = ServiceType;
			newService.ServiceDirectionId = ServiceDirectionId;
			newService.RouteId = RouteId;
			if (ServiceId <= 0)
				{
				ServiceDataAccess.InsertServiceForRoute(newService);
				}
			else
				{
				newService.Id = ServiceId;
				ServiceDataAccess.UpdateService(newService);
				}
			ClearService();
			ServicesUI.ServiceList = new BindableCollection<ServiceModel>(ServiceDataAccess.GetServicesPerRoute(RouteId));
			NotifyOfPropertyChange(() => ServicesUI);
			NotifyOfPropertyChange(()=>CanLoadTimeEvents);
			}

		public void ClearService()
			{
			ServiceName = "";
			ServiceAbbreviation = "";
			ServiceType = "";
			ServiceDescription = "";
			CalculatedDuration = 0;
			ServiceDirectionId = 0;
			ServiceDirectionName = "";
			ServiceId = 0;
			}

		public bool CanLoadTimeEvents
			{
			get
				{
				return SelectedService != null && (FullTimeEventsList==null ||
						FullTimeEventsList.Count == 0);
				}
			}

		public void LoadTimeEvents()
			{
			HasTimeEvents = false;
			FullTimeEventsList = new BindableCollection<FullTimeEventModel>(FullTimeEventDataAccess.CreateTimeEventsPerService(SelectedService.Id));
			NotifyOfPropertyChange(() => FullTimeEventsList);
			}

		public void SaveTimeEvents()
			{
				foreach (var item in FullTimeEventsList)
					{
					if (item.EventType?.Length > 0)
						{
						var timeEvent = new TimeEventModel();
						timeEvent.Id = item.Id;
						timeEvent.EventType = item.EventType;
						timeEvent.ArrivalTime = item.ArrivalTime;
						timeEvent.WaitTime = item.WaitTime;
						timeEvent.LocationId = item.LocationId;
						timeEvent.ServiceId = item.ServiceId;
						timeEvent.Order = item.Order;
					if(item.Id>0)
						{
						timeEvent.Id=item.Id;
						TimeEventDataAccess.UpdateTimeEvent(timeEvent);
						}
					else
						{ 
						TimeEventDataAccess.InsertTimeEventForService(timeEvent);
						}
					}
				}
		  int duration = FullTimeEventsList.Sum(x => x.ArrivalTime+x.WaitTime);
			SelectedService.CalculatedDuration= duration;
      ServiceDataAccess.UpdateServiceCalculatedDuration(duration,SelectedService.Id);
			NotifyOfPropertyChange(()=>ServicesUI.ServiceList);
			FullTimeEventsList= new BindableCollection<FullTimeEventModel>(FullTimeEventDataAccess.GetAllFullTimeEventsPerService(SelectedService.Id));
			Log.Trace($"Time events for service {SelectedService.ServiceAbbreviation} saved", LogEventType.Event);
			}
		}
	}
