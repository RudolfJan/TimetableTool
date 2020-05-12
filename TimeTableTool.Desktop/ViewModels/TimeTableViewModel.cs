using DataAccess.Library.Logic;
using DataAccess.Library.Models;
using System.ComponentModel;
using TimetableTool.Desktop.Models;
using Caliburn.Micro;
using System.Configuration;
using TimetableTool.Desktop.EventModels;

namespace TimetableTool.Desktop.ViewModels
	{
	public class TimetableViewModel : Screen
		{
		private string _TimetableName;
		private string _TimetableAbbreviation;
		private TimetableModel _selectedTimetable;
		private string _timetableDescription;
		private readonly IEventAggregator _events;

		#region Properties
		public TimetableUIModel TimetablesUI { get; set; } = new TimetableUIModel();
		public int RouteId { get; set; } = -1;
		public int TimetableId { get; set; } = -1;
		public string ServiceDirectionName
			{
			get
				{
				return _serviceDirectionName;
				}
			set
				{
				_serviceDirectionName = value;
				NotifyOfPropertyChange(() => ServiceDirectionName);
				}
			}

		public int ServiceDirectionId { get; set; } = -1;
		private ServiceDirectionModel _selectedServiceDirection;

		private bool _isMultiDirection;

		public bool IsMultiDirection
			{
			get
				{
				return _isMultiDirection;
				}
			set
				{
				_isMultiDirection = value;
				NotifyOfPropertyChange(() => IsMultiDirection);
				}
			}

		private ServiceDirectionModel _serviceDirection;
		public ServiceDirectionModel ServiceDirection
			{
			get { return _serviceDirection; }
			set
				{
				_serviceDirection = value;
				NotifyOfPropertyChange(() => ServiceDirection);
				NotifyOfPropertyChange(() => CanSaveTimetable);
				}
			}

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
				if (SelectedTimetable != null)
					{
					if(SelectedTimetable.IsMultiDirection)
						{
						ServiceSourceList = new BindableCollection<ServiceModel>(ServicesDataAccess.GetServicesPerRoute(RouteId));
						}
					else
						{
						ServiceSourceList = new BindableCollection<ServiceModel>(ServicesDataAccess.GetServicesPerRoute(RouteId,SelectedTimetable.ServiceDirectionId));
						}
					ServiceDestinationList = new BindableCollection<ServiceModel>(ServicesDataAccess.GetServicesPerTimetable(SelectedTimetable.Id));
					}
				NotifyOfPropertyChange(() => SelectedTimetable);
				NotifyOfPropertyChange(() => CanEditTimetable);
				NotifyOfPropertyChange(() => CanDeleteTimetable);
				NotifyOfPropertyChange(() => CanAddService);
				NotifyOfPropertyChange(() => CanRemoveService);
				NotifyOfPropertyChange(() => CanCopyAllServices);
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
		private string _serviceDirectionName;

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

		private BindableCollection<ServiceModel> _servicesSourceList;

		public BindableCollection<ServiceModel> ServiceSourceList
			{
			get { return _servicesSourceList; }
			set
				{
				_servicesSourceList = value;
				NotifyOfPropertyChange(() => ServiceSourceList);
				NotifyOfPropertyChange(() => CanCopyAllServices);
				}
			}

		private ServiceModel _selectedServiceSource;

		public ServiceModel SelectedServiceSource
			{
			get
				{
				return _selectedServiceSource;
				}
			set
				{
				_selectedServiceSource = value;
				NotifyOfPropertyChange(() => SelectedServiceSource);
				NotifyOfPropertyChange(() => CanAddService);
				NotifyOfPropertyChange(() => CopyStatus);
				}
			}


		private BindableCollection<ServiceModel> _serviceDestinationList;

		public BindableCollection<ServiceModel> ServiceDestinationList
			{
			get { return _serviceDestinationList; }
			set
				{
				_serviceDestinationList = value;
				NotifyOfPropertyChange(() => ServiceDestinationList);
				NotifyOfPropertyChange(() => CopyStatus);
				}
			}

		private ServiceModel _selectedServiceDestination;

		public ServiceModel SelectedServiceDestination
			{
			get { return _selectedServiceDestination; }
			set
				{
				_selectedServiceDestination = value;
				NotifyOfPropertyChange(() => SelectedServiceDestination);
				NotifyOfPropertyChange(() => CanRemoveService);
				}
			}

		public string CopyStatus 
			{
			get
				{
				var output="";
				if(ServiceSourceList!=null)
					{
					output += $"Nr of services available: {ServiceSourceList.Count}. ";
					}
				else
					{
					output +="No services available to move. ";
					}
				if(ServiceDestinationList!=null)
					{
					output += $"Nr of services in timetable: {ServiceDestinationList.Count}.";
					}
				else
					{
					output +="No services visible in timetable.";
					}
				return output;
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
			TimetablesUI.TimetableList = new BindableCollection<TimetableModel>(TimetableDataAccess.GetAllTimetablesPerRoute(RouteId));
			ServiceDirectionList = new BindableCollection<ServiceDirectionModel>(ServiceDirectionDataAccess.GetAllServiceDirectionsPerRoute(RouteId));
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
			IsMultiDirection = SelectedTimetable.IsMultiDirection;
			if (IsMultiDirection)
				{
				ServiceDirectionId = -1;
				ServiceDirection = null;
				ServiceDirectionName = "";
				}
			else
				{
				ServiceDirectionId = SelectedTimetable.ServiceDirectionId;
				ServiceDirection = ServiceDirectionDataAccess.GetServiceDirectionById(ServiceDirectionId);
				ServiceDirectionName = ServiceDirection.ServiceDirectionName;
				}

			}

		public bool CanDeleteTimetable
			{
			get { return SelectedTimetable != null && Settings.DatabaseVersion>=2; }
			}

		public void DeleteTimetable()
			{
			TimetableDataAccess.DeleteTimetable(SelectedTimetable.Id);
			ServiceSourceList.Clear();
			ServiceDestinationList.Clear();
			TimetablesUI.TimetableList.Remove(SelectedTimetable);
			TimetableId = 0;
			}

		public bool CanSaveTimetable
			{
			get
				{
				return TimetableDescription?.Length > 0
							 && TimetableName?.Length > 0
							 && TimetableAbbreviation?.Length > 0
							 && (IsMultiDirection || ServiceDirectionId > 0);
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
			NotifyOfPropertyChange(() => CanSaveTimetable);
			}

		public void SaveTimetable()
			{
			var newTimetable = new TimetableModel();
			newTimetable.TimetableDescription = TimetableDescription;
			newTimetable.TimetableName = TimetableName;
			newTimetable.TimetableAbbreviation = TimetableAbbreviation;
			newTimetable.RouteId = RouteId;
			newTimetable.IsMultiDirection = IsMultiDirection;
			if (IsMultiDirection)
				{
				newTimetable.ServiceDirectionId = -1;
				}
			else
				{
				newTimetable.ServiceDirectionId = ServiceDirectionId;
				}
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
			TimetablesUI.TimetableList = new BindableCollection<TimetableModel>(TimetableDataAccess.GetAllTimetablesPerRoute(RouteId));
			NotifyOfPropertyChange(() => TimetablesUI);
			}

		public void ClearTimetable()
			{
			TimetableDescription = "";
			TimetableAbbreviation = "";
			TimetableName = "";
			TimetableId = 0;
			IsMultiDirection = false;
			ServiceDirectionName = "";
			ServiceDirectionId = 0;
			ServiceDirection = null;
			}

		public bool CanAddService
			{
			get
				{
				return SelectedServiceSource != null && SelectedTimetable != null;
				}
			}

		public void AddService()
			{
			ServiceDestinationList.Add(SelectedServiceSource);
			ConnectTtSiDataAccess.InsertConnection(SelectedServiceSource.Id, SelectedTimetable.Id);
			NotifyOfPropertyChange(() => ServiceDestinationList);
			NotifyOfPropertyChange(()=>CopyStatus);
			}

		public bool CanRemoveService
			{
			get
				{
				return SelectedServiceDestination != null && SelectedTimetable != null;
				}
			}

		public void RemoveService()
			{
			ConnectTtSiDataAccess.DeleteConnection(SelectedServiceDestination.Id, SelectedTimetable.Id);
			ServiceDestinationList.Remove(SelectedServiceDestination);
			NotifyOfPropertyChange(() => ServiceDestinationList);
			NotifyOfPropertyChange(()=>CopyStatus);
			}


		public bool CanCopyAllServices 
			{ 
			get
				{
				return ServiceSourceList!=null && SelectedTimetable!=null;
				}
			}
		public void CopyAllServices()
			{
			foreach(var item in ServiceSourceList)
				{
				ServiceDestinationList.Add(item);
				ConnectTtSiDataAccess.InsertConnection(item.Id, SelectedTimetable.Id);
				NotifyOfPropertyChange(() => ServiceDestinationList);
				NotifyOfPropertyChange(()=>CopyStatus);
				}
			}
		}
	}
