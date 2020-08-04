using Caliburn.Micro;
using DataAccess.Library.Logic;
using DataAccess.Library.Models;
using System.Linq;
using TimetableTool.Desktop.Models;
using TimetableTool.Desktop.Views;

namespace TimetableTool.Desktop.ViewModels
	{
	public class DisplayTimetableGraphViewModel : Screen
		{
		public int TimetableId { get; set; }

		private TimetableModel _timetable;
		public TimetableModel Timetable
			{
			get { return _timetable; }
			set { _timetable = value; }
			}

		private BindableCollection<LocationModel> _locationList = new BindableCollection<LocationModel>();
		public BindableCollection<LocationModel> LocationList
			{
			get { return _locationList; }
			set
				{
				_locationList = value; 
				NotifyOfPropertyChange(()=>LocationList);
				}	
			}

		// This is a trick to tell the code behind that the collection has changed
		private int _Dummy=1;

		public int Dummy
			{
			get { return _Dummy; }
			set
				{
				_Dummy = value;
				NotifyOfPropertyChange(()=>Dummy);
				}
			}

		private int _timeGraphUIChanged = 1;

		public int TimeGraphUIChanged
			{
			get { return _timeGraphUIChanged; }
			set
				{
				_timeGraphUIChanged = value;
				NotifyOfPropertyChange(()=>TimeGraphUIChanged);
				}
			}


		private BindableCollection<TimeGraphUIModel> _timeGraphUI = new BindableCollection<TimeGraphUIModel>();
		public BindableCollection<TimeGraphUIModel> TimeGraphUI
			{
			get { return _timeGraphUI; }
			set
				{
				_timeGraphUI = value;

				NotifyOfPropertyChange(()=> TimeGraphUI);
				}
			}

	protected override void OnViewLoaded(object view)
			{
			base.OnViewLoaded(view);
			Timetable = TimetableDataAccess.GetTimetableById(TimetableId);
			LocationList =
				new BindableCollection<LocationModel>(
					LocationDataAccess.GetAllLocationsPerRoute(Timetable.RouteId)
						.OrderBy(x => x.Order)
						.ToList());
			int i = 0;
			foreach (var item in LocationList)
				{
				item.Order = i++;
				}


			SetPeriod(); // sets start and end time at the graph
			Dummy += 1;
			LocationList.Refresh();
			PrepareDataSet();
			TimeGraphUIChanged++;
			TimeGraphUI.Refresh();
			}

		private void SetPeriod()
			{			
			var Services = new BindableCollection<ServiceModel>(ServicesDataAccess.GetServicesPerTimetable(TimetableId));
			GraphCanvasSettings.StartTime = (Services.Min(x => x.StartTime)/60)*60;
			GraphCanvasSettings.EndTime=(Services.Max(x => x.EndTime)/60+1)*60;
			}


		private void PrepareDataSet()
			{
			TimeGraphUI= new BindableCollection<TimeGraphUIModel>();
			var serviceList = new BindableCollection<ServiceModel>(ServicesDataAccess.GetServicesPerTimetable(TimetableId));
			foreach (var service in serviceList)
				{
				var item= new TimeGraphUIModel();
				var serviceTemplateId = service.ServiceTemplateId;
				item.TimeEventList= new BindableCollection<ExtendedFullTimeEventModel>(FullTimeEventDataAccess.GetAllExtendedFullTimeEventsPerServiceTemplate(serviceTemplateId));
				item.ServiceName = service.ServiceName;
				item.ServiceAbbreviation = service.ServiceAbbreviation;
				int actualTime = service.StartTime;
				foreach (var fullTimeEvent in item.TimeEventList)
					{
					actualTime += fullTimeEvent.ArrivalTime;
					DataPoint point = GetFirstDataPoint(fullTimeEvent, actualTime);
					item.DataLine.Add(point);
					if (fullTimeEvent.WaitTime > 0)
						{
						actualTime += fullTimeEvent.WaitTime;
						DataPoint point2 = GetSecondDataPoint(actualTime, point.X);
						item.DataLine.Add(point2);
						}
					}
				TimeGraphUI.Add(item);
				}
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

