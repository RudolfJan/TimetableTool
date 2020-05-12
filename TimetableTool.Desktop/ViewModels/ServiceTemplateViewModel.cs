using Caliburn.Micro;
using DataAccess.Library.Logic;
using DataAccess.Library.Models;
using Logging.Library;
using System.Linq;
using TimetableTool.Desktop.EventModels;
using TimetableTool.Desktop.Models;

namespace TimetableTool.Desktop.ViewModels
	{
	public class ServiceTemplateViewModel : Screen
		{
		private readonly IEventAggregator _events;

		#region properties
		public ServiceTemplateUIModel ServiceTemplateUI { get; set; } = new ServiceTemplateUIModel();
		public int RouteId { get; set; } = -1;
		public int ServiceTemplateId { get; set; } = -1;
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

		private ServiceTemplateModel _selectedServiceTemplate;
		private ServiceDirectionModel _selectedServiceDirection;
		private string _serviceTemplateName;
		private string _serviceTemplateDescription;
		private string _serviceTemplateAbbreviation;
		private string _serviceType;
		private int _calculatedDuration;

		public ServiceTemplateModel SelectedServiceTemplate
			{
			get { return _selectedServiceTemplate; }
			set
				{
				_selectedServiceTemplate = value;
				ServiceTemplateSelectedEvent serviceSelectedEvent = new ServiceTemplateSelectedEvent();
				serviceSelectedEvent.SelectedServiceTemplate = _selectedServiceTemplate;
				_events.PublishOnUIThreadAsync(serviceSelectedEvent);
				if (SelectedServiceTemplate != null)
					{
					FullTimeEventsList = new BindableCollection<FullTimeEventModel>(FullTimeEventDataAccess.GetAllFullTimeEventsPerServiceTemplate(SelectedServiceTemplate.Id));
					}
				else
					{
					FullTimeEventsList = null;
					}
				NotifyOfPropertyChange(() => CanLoadTimeEvents);
				NotifyOfPropertyChange(() => SelectedServiceTemplate);
				NotifyOfPropertyChange(() => CanEditServiceTemplate);
				NotifyOfPropertyChange(() => CanDeleteServiceTemplate);
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

		public string ServiceTemplateName
			{
			get
				{
				return _serviceTemplateName;
				}
			set
				{
				_serviceTemplateName = value;
				NotifyOfPropertyChange(() => ServiceTemplateName);
				NotifyOfPropertyChange(() => CanSaveServiceTemplate);
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

		public string ServiceTemplateDescription
			{
			get
				{
				return _serviceTemplateDescription;
				}
			set
				{
				_serviceTemplateDescription = value;
				NotifyOfPropertyChange(() => ServiceTemplateDescription);
				NotifyOfPropertyChange(() => CanSaveServiceTemplate);
				}
			}

		public string ServiceTemplateAbbreviation
			{
			get
				{
				return _serviceTemplateAbbreviation;
				}
			set
				{
				_serviceTemplateAbbreviation = value;
				NotifyOfPropertyChange(() => ServiceTemplateAbbreviation);
				NotifyOfPropertyChange(() => CanSaveServiceTemplate);
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
				NotifyOfPropertyChange(() => CanSaveServiceTemplate);
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
				NotifyOfPropertyChange(() => CanSaveServiceTemplate);
				}
			}

		#endregion

		public ServiceTemplateViewModel(IEventAggregator events)
			{
			_events = events;
			}

		protected override async void OnViewLoaded(object view)
			{
			base.OnViewLoaded(view);
			RouteModel rm = RouteDataAccess.GetRouteById(RouteId);
			ServiceTemplateUI.RouteName = rm.RouteName;
			RouteId = rm.Id;
			ServiceTemplateUI.ServiceTemplateList = new BindableCollection<ServiceTemplateModel>(ServiceTemplateDataAccess.GetServiceTemplatesPerRoute(RouteId));
			ServiceDirectionList = new BindableCollection<ServiceDirectionModel>(ServiceDirectionDataAccess.GetAllServiceDirectionsPerRoute(RouteId));
			NotifyOfPropertyChange(() => ServiceTemplateUI);
			NotifyOfPropertyChange(() => ServiceDirectionList);
			}

		public bool CanEditServiceTemplate
			{
			get { return SelectedServiceTemplate != null && ServiceTemplateId <= 0; }
			}

		public void EditServiceTemplate()
			{
			ServiceTemplateName = SelectedServiceTemplate.ServiceTemplateName;
			ServiceTemplateAbbreviation = SelectedServiceTemplate.ServiceTemplateAbbreviation;
			ServiceType = SelectedServiceTemplate.ServiceType;
			ServiceTemplateDescription = SelectedServiceTemplate.ServiceTemplateDescription;
			CalculatedDuration = SelectedServiceTemplate.CalculatedDuration;
			ServiceDirectionId = SelectedServiceTemplate.ServiceDirectionId;
			SelectedServiceDirection = ServiceDirectionDataAccess.GetServiceDirectionById(ServiceDirectionId);
			ServiceDirectionName = SelectedServiceDirection.ServiceDirectionName;
			ServiceTemplateId = SelectedServiceTemplate.Id;
			FullTimeEventsList = new BindableCollection<FullTimeEventModel>(FullTimeEventDataAccess.GetAllFullTimeEventsPerServiceTemplate(SelectedServiceTemplate.Id));
			NotifyOfPropertyChange(() => CanLoadTimeEvents);
			NotifyOfPropertyChange(() => CanEditServiceTemplate);
			NotifyOfPropertyChange(() => CanDeleteServiceTemplate);

			}

		public bool CanDeleteServiceTemplate
			{
			get { return SelectedServiceTemplate != null && Settings.DatabaseVersion >= 2; }
			}
		public bool CanSaveServiceTemplate
			{
			get
				{
				return ServiceTemplateAbbreviation?.Length > 0
							 && ServiceTemplateName?.Length > 0
							 && ServiceTemplateDescription?.Length > 0
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
			NotifyOfPropertyChange(() => CanSaveServiceTemplate);
			NotifyOfPropertyChange(() => ServiceDirectionName);
			}

		public void SaveServiceTemplate()
			{
			var newService = new ServiceTemplateModel();
			newService.CalculatedDuration = CalculatedDuration;
			newService.ServiceTemplateAbbreviation = ServiceTemplateAbbreviation;
			newService.ServiceTemplateDescription = ServiceTemplateDescription;
			newService.ServiceTemplateName = ServiceTemplateName;
			newService.ServiceType = ServiceType;
			newService.ServiceDirectionId = ServiceDirectionId;
			newService.RouteId = RouteId;
			if (ServiceTemplateId <= 0)
				{
				ServiceTemplateDataAccess.InsertServiceTemplate(newService);
				}
			else
				{
				newService.Id = ServiceTemplateId;
				ServiceTemplateDataAccess.UpdateServiceTemplate(newService);
				}
			ServiceTemplateUI.ServiceTemplateList = new BindableCollection<ServiceTemplateModel>(ServiceTemplateDataAccess.GetServiceTemplatesPerRoute(RouteId));
			ClearServiceTemplate();
  		}

		public void ClearServiceTemplate()
			{
			ServiceTemplateName = "";
			ServiceTemplateAbbreviation = "";
			ServiceType = "";
			ServiceTemplateDescription = "";
			CalculatedDuration = 0;
			ServiceDirectionId = 0;
			ServiceDirectionName = "";
			ServiceTemplateId = 0;
			NotifyOfPropertyChange(() => ServiceTemplateUI);
			NotifyOfPropertyChange(() => CanLoadTimeEvents);
			NotifyOfPropertyChange(() => CanEditServiceTemplate);
			}

		public bool CanLoadTimeEvents
			{
			get
				{
				return SelectedServiceTemplate != null && (FullTimeEventsList == null ||
						FullTimeEventsList.Count == 0);
				}
			}

		public void LoadTimeEvents()
			{
			HasTimeEvents = false;
			FullTimeEventsList = new BindableCollection<FullTimeEventModel>(FullTimeEventDataAccess.CreateTimeEventsPerServiceTemplate(SelectedServiceTemplate.Id));
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
					timeEvent.ServiceTemplateId = item.ServiceTemplateId;
					timeEvent.Order = item.Order;
					if (item.Id > 0)
						{
						timeEvent.Id = item.Id;
						TimeEventDataAccess.UpdateTimeEvent(timeEvent);
						}
					else
						{
						TimeEventDataAccess.InsertTimeEventForServiceTemplate(timeEvent);
						}
					}
				}
			int duration = FullTimeEventsList.Sum(x => x.ArrivalTime + x.WaitTime);
			SelectedServiceTemplate.CalculatedDuration = duration;
			ServiceTemplateDataAccess.UpdateServiceCalculatedDuration(duration, SelectedServiceTemplate.Id);
			ServiceTemplateUI.ServiceTemplateList.Refresh();
			FullTimeEventsList = new BindableCollection<FullTimeEventModel>(FullTimeEventDataAccess.GetAllFullTimeEventsPerServiceTemplate(SelectedServiceTemplate.Id));
			Log.Trace($"Time events for service {SelectedServiceTemplate.ServiceTemplateAbbreviation} saved", LogEventType.Event);
			}

		public void DeleteServiceTemplate()
			{
			ServiceTemplateDataAccess.DeleteServiceTemplate(SelectedServiceTemplate.Id);
			ServiceTemplateUI.ServiceTemplateList.Remove(SelectedServiceTemplate);
			ServiceTemplateId = 0;
			}
		}
	}
