using Caliburn.Micro;
using DataAccess.Library.Logic;
using DataAccess.Library.Models;
using Logging.Library;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using TimetableTool.Desktop.EventModels;
using TimetableTool.Desktop.Helpers;
using TimetableTool.Desktop.Models;
using TimetableTool.Desktop.Views;

namespace TimetableTool.Desktop.ViewModels
	{
	public class ShellViewModel : Conductor<object>, IHandle<RouteSelectedEvent>, IHandle<ServiceTemplateSelectedEvent>, IHandle<TimetableSelectedEvent>, IHandle<ReportSelectedEvent>, IHandle<TrainSelectedEvent>
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

		public bool IsEditServiceTemplatesEnabled
			{
			get { return SelectedRoute != null; }
			}

		public bool IsEditTimeEventsEnabled
			{
			get
				{
				return SelectedRoute != null && SelectedServiceTemplate != null;
				}
			}

		public bool IsEditServicesEnabled
			{
			get
				{
				return SelectedRoute != null;
				}
			}

		public bool IsEditTrainsEnabled
			{
			get
				{
				return SelectedRoute != null;
				}
			}


		public bool IsEditTrainServiceEnabled
			{
			get
				{
				return SelectedTrain != null;
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
				NotifyOfPropertyChange(() => IsEditServiceTemplatesEnabled);
				NotifyOfPropertyChange(() => IsEditTimeEventsEnabled);
				NotifyOfPropertyChange(() => IsEditServicesEnabled);
				NotifyOfPropertyChange(() => IsEditServiceDirectionsEnabled);
				NotifyOfPropertyChange(()=> IsEditTrainsEnabled);
				}
			}

		private ServiceTemplateModel _selectedServiceTemplate;
		public ServiceTemplateModel SelectedServiceTemplate
			{
			get { return _selectedServiceTemplate; }
			set
				{
				_selectedServiceTemplate = value;
				NotifyOfPropertyChange(() => SelectedServiceTemplate);
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
				NotifyOfPropertyChange(() => IsEditServicesEnabled);
				NotifyOfPropertyChange(()=>IsTimetableSelected);
				NotifyOfPropertyChange(()=>IsTimetableSelectedMultiDirection);
				}
			}

		private TrainModel _selectedTrain;

		public TrainModel SelectedTrain
			{
			get
				{
				return _selectedTrain;
				}
			set
				{
				_selectedTrain = value;
				NotifyOfPropertyChange(()=>IsEditTrainServiceEnabled);
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

		public async Task Reports()
			{
			var reportsVM = IoC.Get<ReportsViewModel>();
			await ActivateItemAsync(reportsVM, new CancellationToken());
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


		public async Task ViewArrivalDeparture()
			{
			var departureArrivalVM = IoC.Get<DepartureArrivalTimetableViewModel>();
			departureArrivalVM.RouteId = SelectedRoute.Id;
			departureArrivalVM.TimetableId = SelectedTimetable.Id;
			await ActivateItemAsync(departureArrivalVM, new CancellationToken());
			}

		public async Task Backup()
			{
			var backupVM = IoC.Get<BackupViewModel>();
			await ActivateItemAsync( backupVM, new CancellationToken());
			}

		public async Task SettingsMenu()
			{
			var SettingsVM = IoC.Get<SettingsViewModel>();
			await _windowManager.ShowDialogAsync(SettingsVM);
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

		public async Task EditServiceTemplates()
			{
			var serviceTemplatesVM = IoC.Get<ServiceTemplateViewModel>();
			serviceTemplatesVM.RouteId = SelectedRoute.Id;
			await ActivateItemAsync(serviceTemplatesVM, new CancellationToken());
			}

		public async Task EditTimeEvents()
			{
			var timeEventsVM = IoC.Get<TimeEventViewModel>();
			timeEventsVM.RouteId = SelectedRoute.Id;
			timeEventsVM.ServiceId = SelectedServiceTemplate.Id;
			await ActivateItemAsync(timeEventsVM, new CancellationToken());
			}


		public async Task EditServices()
			{
			var serviceVM = IoC.Get<ServiceViewModel>();
			serviceVM.RouteId = SelectedRoute.Id;
			await ActivateItemAsync(serviceVM, new CancellationToken());
			}

		public async Task EditTrains()
			{
			var trainsVM = IoC.Get<TrainViewModel>();
			trainsVM.Route = SelectedRoute;
			await ActivateItemAsync(trainsVM, new CancellationToken());
			}

		public async Task EditTrainServices()
			{
			var trainServiceVM = IoC.Get<TrainServiceViewModel>();
			trainServiceVM.Train = SelectedTrain;
			await ActivateItemAsync(trainServiceVM, new CancellationToken());
			}
		public async Task ShowAbout()
			{
			await _windowManager.ShowDialogAsync(IoC.Get<AboutViewModel>());
			}

		public void ShowManual()
			{
			try
				{
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

		public Task HandleAsync(ServiceTemplateSelectedEvent message, CancellationToken cancellationToken)
			{
			SelectedServiceTemplate = message.SelectedServiceTemplate;
			return Task.CompletedTask;
			}

		public Task HandleAsync(TrainSelectedEvent message, CancellationToken cancellationToken)
			{
			SelectedTrain = message.SelectedTrain;
			NotifyOfPropertyChange(()=> IsEditTrainServiceEnabled);
			return Task.CompletedTask;
			}
		public Task HandleAsync(RouteSelectedEvent message, CancellationToken cancellationToken)
			{
			// If you change the selected route, you must reselect the service, because it is attached to the route.
			if (message.SelectedRoute!=null && SelectedRoute?.Id != message.SelectedRoute.Id)
				{
				SelectedServiceTemplate = null;
				SelectedTimetable = null;
				NotifyOfPropertyChange(() => IsEditTimeEventsEnabled);
				NotifyOfPropertyChange(() => IsEditServicesEnabled);
				}
			SelectedRoute = message.SelectedRoute;
			return Task.CompletedTask;
			}

		Task IHandle<TimetableSelectedEvent>.HandleAsync(TimetableSelectedEvent message, CancellationToken cancellationToken)
			{
			SelectedTimetable = message.SelectedTimetable;
			return Task.CompletedTask;
			}

		async Task IHandle<ReportSelectedEvent>.HandleAsync(ReportSelectedEvent message, CancellationToken cancellationToken)
			{
			switch (message.Report)
				{
					case ReportType.Classic:
						{
						var displayTimetableVM = IoC.Get<DisplayTimetableViewModel>();
						displayTimetableVM.TimetableId = message.SelectedTimetable.TimetableId;
						await ActivateItemAsync(displayTimetableVM, new CancellationToken());
						break;
						}
					case ReportType.Graph:
						{
						var displayTimetableGraphVM = IoC.Get<DisplayTimetableGraphViewModel>();
						displayTimetableGraphVM.TimetableId = message.SelectedTimetable.TimetableId;
						await ActivateItemAsync(displayTimetableGraphVM, new CancellationToken());
						break;
						}
					case ReportType.ScottPlotGraph:
						{
						var displayScottPlotGraphVM =
							new ScottPlotGraph(message.SelectedTimetable.TimetableId);
						var form= new ScottPlotGraphForm(displayScottPlotGraphVM);
						form.Show();
						break;
						}

					case ReportType.ArrivalDeparture:
						{
						var departureArrivalVM = IoC.Get<DepartureArrivalTimetableViewModel>();
						departureArrivalVM.RouteId = message.SelectedTimetable.RouteId;
						departureArrivalVM.TimetableId = message.SelectedTimetable.TimetableId;
						await ActivateItemAsync(departureArrivalVM, new CancellationToken());
						break;
						}
					case ReportType.TrainPlanning:
						{
						var displayTrainPlanningVM = new TrainServiceGraph(message.SelectedTimetable.RouteId);
						var form= new TrainServiceGraphForm(displayTrainPlanningVM);
						form.Show();
						break;
						}
					case ReportType.ConsistencyChecks:
						{
						var statisticsVM = new StatisticsGraph();
						var form = new StatisticsGraphForm(statisticsVM);
						form.Show();
						break;
						}
					default:
						{
						throw new NotImplementedException("Report type not implemented in ShellView");
						}
				}
			}


		#endregion
		}
	}
