using Caliburn.Micro;
using DataAccess.Library.Logic;
using DataAccess.Library.Models;
using Logging.Library;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TimetableTool.Desktop.EventModels;
using TimetableTool.Desktop.Helpers;
using TimetableTool.Desktop.Models;

namespace TimetableTool.Desktop.ViewModels
	{
	public class ShellViewModel : Conductor<object>, IHandle<RouteSelectedEvent>, IHandle<ServiceSelectedEvent>, IHandle<TimetableSelectedEvent>
	//, IHandle<LocationSelectedEvent>, IHandle<ServiceSelectedEvent>
		{
		private readonly IEventAggregator _events;
		private readonly IWindowManager _windowManager;

		public bool IsEditLocationsEnabled
			{
			get { return SelectedRoute != null; }
			}

		public bool IsEditServiceDirectionsEnabled
			{
			get 
				{ 
				return SelectedRoute != null;
				}
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
				return SelectedRoute != null && SelectedService != null;
				}
			}

		public bool IsEditServiceInstancesEnabled
			{
			get
				{
				return SelectedRoute != null;
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
				NotifyOfPropertyChange(() => SelectedRoute);
				NotifyOfPropertyChange(() => IsEditLocationsEnabled);
				NotifyOfPropertyChange(() => IsEditTimetablesEnabled);
				NotifyOfPropertyChange(() => IsEditServicesEnabled);
				NotifyOfPropertyChange(() => IsEditTimeEventsEnabled);
				NotifyOfPropertyChange(() => IsEditServiceInstancesEnabled);
				NotifyOfPropertyChange(() => IsEditServiceDirectionsEnabled);
				}
			}

		private ServiceModel _selectedService;
		public ServiceModel SelectedService
			{
			get { return _selectedService; }
			set
				{
				_selectedService = value;
				NotifyOfPropertyChange(() => SelectedService);
				NotifyOfPropertyChange(() => IsEditTimeEventsEnabled);
				}
			}

		private TimetableModel _selectedTimetable;
		public TimetableModel SelectedTimetable
			{
			get { return _selectedTimetable; }
			set
				{
				_selectedTimetable = value;
				// Note for some timetable displays it is assumed they are unidirectional. Therefore we use two conditions here to enable the views.
				IsTimetableSelected= SelectedTimetable != null && SelectedTimetable.IsMultiDirection==false;
				IsTimetableSelectedMultiDirection = SelectedTimetable != null;
				NotifyOfPropertyChange(() => SelectedTimetable);
				NotifyOfPropertyChange(() => IsEditServiceInstancesEnabled);
				NotifyOfPropertyChange(()=>IsTimetableSelected);
				NotifyOfPropertyChange(()=>IsTimetableSelectedMultiDirection);
				}
			}

		public bool IsTimetableSelected { get; set; }
		public bool IsTimetableSelectedMultiDirection { get; set; }

		public SaveFileModel SaveFileParams { get; set; } = new SaveFileModel();

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

		public void ExportTimetable()
			{
			// This one uses the ShellViewModel
			SaveFileParams.Title = "Export timetable as csv file";
			DataAccess.Library.Models.TimetableMatrixModel matrix = TimetableMatrixDataAccess.ReadTimetableMatrix(SelectedTimetable.Id,true);
			var csv = TimetableMatrixDataAccess.GetCsvData(matrix.Matrix);
			if (csv.Length > 0)
				{
				var fileName = FileIOHelper.GetSaveFileName(SaveFileParams);
				if (fileName.Length > 0)
					{
					File.WriteAllText(fileName, csv);
					}
				}
			}

		public async Task ViewTimetable()
			{
			var displayTimetableVM = IoC.Get<DisplayTimetableViewModel>();
			displayTimetableVM.TimetableId = SelectedTimetable.Id;
			await ActivateItemAsync(displayTimetableVM, new CancellationToken());
			}

		public async Task ViewGraph()
			{
			var displayTimetableGraphVM = IoC.Get<DisplayTimetableGraphViewModel>();
			displayTimetableGraphVM.TimetableId = SelectedTimetable.Id;
			await ActivateItemAsync(displayTimetableGraphVM, new CancellationToken());

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

		public async Task EditServiceDirections()
			{
			var serviceDirectionVM = IoC.Get<ServiceDirectionsViewModel>();
			serviceDirectionVM.RouteId = SelectedRoute.Id;
			await ActivateItemAsync(serviceDirectionVM, new CancellationToken());
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


		public async Task EditServiceInstances()
			{
			var serviceInstanceVM = IoC.Get<ServiceInstanceViewModel>();
			serviceInstanceVM.RouteId = SelectedRoute.Id;
			await ActivateItemAsync(serviceInstanceVM, new CancellationToken());
			}

		public async Task ShowAbout()
			{
			await _windowManager.ShowDialogAsync(IoC.Get<AboutViewModel>());
			}

		public void ShowManual()
			{
			try
				{
				Log.Trace($"Manual path:{Settings.ManualPath}", LogEventType.Event);
				if(File.Exists(Settings.ManualPath))
					{
					FileIOHelper.OpenFileWithShell(Settings.ManualPath);
					}
				else
					{
					Log.Trace($"Cannot open {Settings.ManualPath}. Check if the manual is available there", LogEventType.Error);
					}
				}
			catch(Exception ex)
				{
				Log.Trace($"Cannot open {Settings.ManualPath}. Check if the manual is available there",ex, LogEventType.Error);
				}
			}
		public async Task ShowLogging()
			{
			await _windowManager.ShowWindowAsync(IoC.Get<LoggingViewModel>());
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
				SelectedTimetable = null;
				NotifyOfPropertyChange(() => IsEditTimeEventsEnabled);
				NotifyOfPropertyChange(() => IsEditServiceInstancesEnabled);
				}
			SelectedRoute = message.SelectedRoute;
			return Task.CompletedTask;
			}

		Task IHandle<TimetableSelectedEvent>.HandleAsync(TimetableSelectedEvent message, CancellationToken cancellationToken)
			{
			SelectedTimetable = message.SelectedTimetable;
			return Task.CompletedTask;
			}
		#endregion
		}
	}
