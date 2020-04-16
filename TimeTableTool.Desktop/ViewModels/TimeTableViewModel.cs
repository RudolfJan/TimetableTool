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
					ServiceInstancesDestinationList = new BindableCollection<ServiceInstanceModel>(ServiceInstanceDataAccess.GetServiceInstancesPerTimetable(SelectedTimetable.Id));
					}
				NotifyOfPropertyChange(() => SelectedTimetable);
				NotifyOfPropertyChange(() => CanEditTimetable);
				NotifyOfPropertyChange(() => CanDeleteTimetable);
				NotifyOfPropertyChange(() => CanAddServiceInstance);
				NotifyOfPropertyChange(() => CanRemoveServiceInstance);
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

		private BindableCollection<ServiceInstanceModel> _serviceInstancesSourceList;

		public BindableCollection<ServiceInstanceModel> ServiceInstancesSourceList
			{
			get { return _serviceInstancesSourceList; }
			set
				{
				_serviceInstancesSourceList = value;
				NotifyOfPropertyChange(() => ServiceInstancesSourceList);
				}
			}

		private ServiceInstanceModel _selectedServiceInstanceSource;

		public ServiceInstanceModel SelectedServiceInstanceSource
			{
			get
				{
				return _selectedServiceInstanceSource;
				}
			set
				{
				_selectedServiceInstanceSource = value;
				NotifyOfPropertyChange(() => SelectedServiceInstanceSource);
				NotifyOfPropertyChange(() => CanAddServiceInstance);
				}
			}


		private BindableCollection<ServiceInstanceModel> _serviceInstancesDestinationList;

		public BindableCollection<ServiceInstanceModel> ServiceInstancesDestinationList
			{
			get { return _serviceInstancesDestinationList; }
			set
				{
				_serviceInstancesDestinationList = value;
				NotifyOfPropertyChange(() => ServiceInstancesDestinationList);
				}
			}

		private ServiceInstanceModel _selectedServiceInstanceDestination;

		public ServiceInstanceModel SelectedServiceInstanceDestination
			{
			get { return _selectedServiceInstanceDestination; }
			set
				{
				_selectedServiceInstanceDestination = value;
				NotifyOfPropertyChange(() => SelectedServiceInstanceDestination);
				NotifyOfPropertyChange(() => CanRemoveServiceInstance);
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

			ServiceInstancesSourceList = new BindableCollection<ServiceInstanceModel>(ServiceInstanceDataAccess.GetServiceInstancesPerRoute(RouteId));
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
			ServiceDirectionId = SelectedTimetable.Id;
			ServiceDirection = ServiceDirectionDataAccess.GetServiceDirectionById(ServiceDirectionId);
			ServiceDirectionName = ServiceDirection.ServiceDirectionName;
			IsMultiDirection = SelectedTimetable.IsMultiDirection;
			}

		public bool CanDeleteTimetable
			{
			get { return false; }
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
			newTimetable.ServiceDirectionId = ServiceDirectionId;
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
			ServiceDirectionName = "";
			ServiceDirectionId = 0;
			ServiceDirection = null;
			}

		public bool CanAddServiceInstance
			{
			get
				{
				return SelectedServiceInstanceSource != null && SelectedTimetable != null;
				}
			}

		public void AddServiceInstance()
			{
			ServiceInstancesDestinationList.Add(SelectedServiceInstanceSource);
			ConnectTtSiDataAccess.InsertConnection(SelectedServiceInstanceSource.Id, SelectedTimetable.Id);
			NotifyOfPropertyChange(() => ServiceInstancesDestinationList);
			}

		public bool CanRemoveServiceInstance
			{
			get
				{
				return SelectedServiceInstanceDestination != null && SelectedTimetable != null;
				}
			}


		public void RemoveServiceInstance()
			{
			ConnectTtSiDataAccess.DeleteConnection(SelectedServiceInstanceDestination.Id, SelectedTimetable.Id);
			ServiceInstancesDestinationList.Remove(SelectedServiceInstanceDestination);
			}
		}
	}
