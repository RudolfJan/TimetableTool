using Caliburn.Micro;
using DataAccess.Library.Logic;
using DataAccess.Library.Models;
using System.Threading.Tasks;
using TimetableTool.Desktop.EventModels;

namespace TimetableTool.Desktop.ViewModels
	{
	public class ReportsViewModel: Screen
		{
		#region Properties
		private readonly IEventAggregator _events;

		private BindableCollection<TimetableRouteModel> _timetableList;

		public BindableCollection<TimetableRouteModel> TimetableList
			{
			get
				{
				return _timetableList;
				}
			set
				{
				_timetableList = value;
				}
			}

		private TimetableRouteModel _selectedTimetable;

		public TimetableRouteModel SelectedTimetable
			{
			get
				{
				return _selectedTimetable;
				}
			set
				{
				_selectedTimetable = value;
				NotifyOfPropertyChange(()=>CanViewArrivalDeparture);
				NotifyOfPropertyChange(()=>CanViewGraph);
				NotifyOfPropertyChange(()=>CanScottPlotViewGraph);
				NotifyOfPropertyChange(()=>CanViewTimetable);
				NotifyOfPropertyChange(() => CanTrainPlanning);
				}
			}


		#endregion

		#region Initialization

		public ReportsViewModel(IEventAggregator events)
			{
			_events = events;
			}

		protected override async void OnViewLoaded(object view)
			{
			base.OnViewLoaded(view);
			TimetableList= new BindableCollection<TimetableRouteModel>(TimetableDataAccess.GetAllTimetables());
			NotifyOfPropertyChange(()=>TimetableList);
			}
		#endregion


		#region ButtonHandlers

		public bool CanViewTimetable
			{
			get
				{
				return SelectedTimetable != null && SelectedTimetable.IsMultiDirection == false;
				}
			}

		public async Task ViewTimetable()
			{
			ReportSelectedEvent reportEvent= new ReportSelectedEvent();
			reportEvent.SelectedTimetable = SelectedTimetable;
			reportEvent.Report = ReportType.Classic;
			await _events.PublishOnUIThreadAsync(reportEvent);
			}

		public bool CanViewGraph
			{
			get
				{
				return SelectedTimetable != null;
				}
			}

		public async Task ViewGraph()
			{
			ReportSelectedEvent reportEvent= new ReportSelectedEvent();
			reportEvent.SelectedTimetable = SelectedTimetable;
			reportEvent.Report = ReportType.Graph;
			await _events.PublishOnUIThreadAsync(reportEvent);
			}

		public bool CanScottPlotViewGraph
			{
			get
				{
				return SelectedTimetable != null;
				}
			}

		public async Task ScottPlotViewGraph()
			{
			ReportSelectedEvent reportEvent= new ReportSelectedEvent();
			reportEvent.SelectedTimetable = SelectedTimetable;
			reportEvent.Report = ReportType.ScottPlotGraph;
			await _events.PublishOnUIThreadAsync(reportEvent);
			}

		public bool CanViewArrivalDeparture
			{
			get
				{
				return SelectedTimetable != null;
				}
			}
		public async Task ViewArrivalDeparture()
			{
			ReportSelectedEvent reportEvent= new ReportSelectedEvent();
			reportEvent.SelectedTimetable = SelectedTimetable;
			reportEvent.Report = ReportType.ArrivalDeparture;
			await _events.PublishOnUIThreadAsync(reportEvent);
			}

		public bool CanTrainPlanning
			{
			get
				{
				return SelectedTimetable != null;
				}
			}


		public async Task TrainPlanning()
			{
			ReportSelectedEvent reportEvent = new ReportSelectedEvent();
			reportEvent.SelectedTimetable = SelectedTimetable;
			reportEvent.Report = ReportType.TrainPlanning;
			await _events.PublishOnUIThreadAsync(reportEvent);
			}
		#endregion

		public async Task ViewConsistencyChecks()
			{
			ReportSelectedEvent reportEvent = new ReportSelectedEvent();
			reportEvent.Report = ReportType.ConsistencyChecks;
			await _events.PublishOnUIThreadAsync(reportEvent);
			}
		}
	}
