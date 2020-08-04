using Caliburn.Micro;
using DataAccess.Library.Logic;
using DataAccess.Library.Models;
using Styles.Library.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TimetableTool.DataAccessLibrary.Logic;
using TimetableTool.Desktop.Models;


// Note: this ViewModel does NOT use Caliburn.Micro in its call. Simply not working. I pass the entire ViewModel to the View.

namespace TimetableTool.Desktop.ViewModels
	{
	public class ScottPlotGraph: Notifier
		{
		public int TimetableId { get; set; }
		public int ScottPlotWidth { get; set; } = Settings.ScottPlotWidth;
		public int ScottPlotHeight { get; set; } = Settings.ScottPlotHeight;

		public string StartTimeText { get; set; } = "00:00";

		public string EndTimeText { get; set; } = "23:59";
		private double _zoom = 1;
		public double Zoom {
			get
				{
				return _zoom;
				}
			set
				{
				_zoom = value;
				OnPropertyChanged("Zoom");
				}
			}

		private double _pan = 0;
		public double Pan {
			get
				{
				return _pan;
				}
			set
				{
				_pan = value;
				OnPropertyChanged("Pan");
				}
			}

		public List<ServiceClassModel> ServiceClassList { get; set; }

		private TimeGraphUIModel _selectedTimeGraph;
		public TimeGraphUIModel SelectedTimeGraph {
			get
				{
				return _selectedTimeGraph;
				}
			set
				{
				_selectedTimeGraph = value;
				OnPropertyChanged("SelectedTimeGraph");
				}
			}

		private TimetableModel _timetable;
		public TimetableModel Timetable
			{
			get { return _timetable; }
			set { _timetable = value; }
			}

		private ObservableCollection<LocationModel> _locationList = new ObservableCollection<LocationModel>();
		public ObservableCollection<LocationModel> LocationList
			{
			get
				{
				return _locationList;
				}
			set
				{
				_locationList = value; 
				OnPropertyChanged("LocationList");
				//NotifyOfPropertyChange(()=>LocationList);
				}	
			}

	  private ObservableCollection<TimeGraphUIModel> _timeGraphUI = new ObservableCollection<TimeGraphUIModel>();
		public ObservableCollection<TimeGraphUIModel> TimeGraphUI
			{
			get { return _timeGraphUI; }
			set
				{
				_timeGraphUI = value;
				OnPropertyChanged("TimeGraphUI");
				// NotifyOfPropertyChange(()=> TimeGraphUI);
				}
			}


		public ScottPlotGraph(int timetableId)
			{
			TimetableId = timetableId;
			Timetable = TimetableDataAccess.GetTimetableById(TimetableId);

			LocationList = new ObservableCollection<LocationModel>(LocationDataAccess.GetAllLocationsPerRoute(Timetable.RouteId)
						.OrderBy(x => x.Order)
						.ToList());
			int i = 0;
			foreach (var item in LocationList)
				{
				item.Order = i++;
				}
			OnPropertyChanged("LocationList");
			PrepareDataSet();
			}

	private void PrepareDataSet()
			{
			TimeGraphUI= new ObservableCollection<TimeGraphUIModel>();
			ServiceClassList = ServiceClassDataAccess.GetAllServiceClasses();
			var serviceList = new ObservableCollection<ServiceModel>(ServicesDataAccess.GetServicesPerTimetable(TimetableId));
			foreach (var service in serviceList)
				{
				var item= new TimeGraphUIModel();
				var serviceTemplateId = service.ServiceTemplateId;
				var serviceTemplate = ServiceTemplateDataAccess.GetServiceTemplateById(serviceTemplateId);
				item.TimeEventList = new BindableCollection<ExtendedFullTimeEventModel>(FullTimeEventDataAccess.GetAllExtendedFullTimeEventsPerServiceTemplate(serviceTemplateId));
				item.ServiceName = service.ServiceName;
				item.ServiceAbbreviation = service.ServiceAbbreviation;
				item.ServiceType = serviceTemplate.ServiceType;
				item.StartTimeText = service.StartTimeText;
				item.EndTimeText = service.EndTimeText;
				int actualTime = service.StartTime;
				foreach (var fullTimeEvent in item.TimeEventList)
					{
					actualTime += fullTimeEvent.ArrivalTime;
					fullTimeEvent.ArrivalTimeText = TimeConverters.MinutesToString(actualTime);
					DataPoint point = GetFirstDataPoint(fullTimeEvent, actualTime);
					item.DataLine.Add(point);
					if (fullTimeEvent.WaitTime > 0)
						{
						actualTime += fullTimeEvent.WaitTime;
						DataPoint point2 = GetSecondDataPoint(actualTime, point.X);
						item.DataLine.Add(point2);
						}
					fullTimeEvent.DepartureTimeText = TimeConverters.MinutesToString(actualTime);
					}
				TimeGraphUI.Add(item);
				}
			OnPropertyChanged("TimeGraphUI");
			}

		private DataPoint GetFirstDataPoint(FullTimeEventModel fullTimeEvent, int actualTime)
			{
			DataPoint output= new DataPoint();
			output.Y = actualTime;
			output.X = LocationList.Where(x=>x.Id== fullTimeEvent.LocationId)
																			.Select(x => x.Order).First();
			
			return output;
			}

		private DataPoint GetSecondDataPoint(int actualTime, double xValue)
			{
			DataPoint output= new DataPoint();
			output.Y = actualTime;
			output.X = xValue;
			return output;
			}
		}
	}
