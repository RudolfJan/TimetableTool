using DataAccess.Library.Logic;
using DataAccess.Library.Models;
using Styles.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms.VisualStyles;
using TimetableTool.DataAccessLibrary.Logic;
using TimetableTool.Desktop.Models;

namespace TimetableTool.Desktop.ViewModels
	{
	public class TrainServiceGraph: Notifier
		{
		public RouteModel Route { get; set; }
		public int ScottPlotWidth { get; set; } = Settings.ScottPlotWidth;
		public int ScottPlotHeight { get; set; } = Settings.ScottPlotHeight;

		public string StartTimeText { get; set; } = "00:00";
		public string EndTimeText { get; set; } = "23:59";

		private List<ServiceClassModel>
			ServiceClassList = ServiceClassDataAccess.GetAllServiceClasses();

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
				}
			}

		private ObservableCollection<TrainPlanningUIModel> _trainPlanning;

		public ObservableCollection<TrainPlanningUIModel> TrainPlanning
			{
			get
				{
				return _trainPlanning;
				}
			set
				{
				_trainPlanning = value;
				OnPropertyChanged("TrainPlanning");
				}
			}


		private TrainPlanningUIModel _selectedTrain;

		public TrainPlanningUIModel SelectedTrain
			{
			get
				{
				return _selectedTrain;
				}
			set
				{
				_selectedTrain = value;
				OnPropertyChanged("SelectedTrain");
				}
			}

		private double _zoom = 1;
		public double Zoom
			{
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
		public double Pan
			{
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



		public TrainServiceGraph(int routeId)
			{
			Route = RouteDataAccess.GetRouteById(routeId);
			LocationList = new ObservableCollection<LocationModel>(LocationDataAccess.GetAllLocationsPerRoute(routeId)
				.OrderBy(x => x.Order)
				.ToList());
			int i = 0;
			foreach (var item in LocationList)
				{
				item.Order = i++;
				}
			OnPropertyChanged("LocationList");
			TrainPlanning=new ObservableCollection<TrainPlanningUIModel>();
			var TrainList = TrainDataAccess.GetAllTrainsPerRoute(routeId);
			foreach (var train in TrainList)
				{
				TrainPlanningUIModel trainUi= new TrainPlanningUIModel();
				trainUi.Train = train;
				var ServiceList= ServicesDataAccess.GetServicesPerRoutePerTrainInTrainServices(routeId, train.Id);
				trainUi.ServicesInTrain = new List<PlottableServiceModel>();
				DataPoint firstPoint = null;
				foreach (var item in ServiceList)
					{
					var plottableService = new PlottableServiceModel(item);
					firstPoint=AddTimeEventsToDataLineForService(firstPoint, plottableService);
					trainUi.ServicesInTrain.Add(plottableService);
					ConvertDataLineToPlotData(plottableService);
					if (trainUi.LegendDataPoint == null)
						{
						trainUi.LegendDataPoint = plottableService.DataLine.First();
						}
					}
				TrainPlanning.Add(trainUi);
				}
			}

		private void ConvertDataLineToPlotData(PlottableServiceModel plottableService)
			{
			int count = plottableService.DataLine.Count;
			plottableService.LocationValue= new double[count];
			plottableService.TimeValue = new double[count];
			int i = 0;
			foreach (var item in plottableService.DataLine)
				{
				plottableService.LocationValue[i] = item.X;
				plottableService.TimeValue[i] = - item.Y; // We need to invert the Y-axis in order to get the annotation in the correct order
				i++;
				} 
			}

		private DataPoint AddTimeEventsToDataLineForService(DataPoint firstPoint,PlottableServiceModel plottableService)
			{
			plottableService.LineColor = GetLineColor(plottableService.ServiceType, ServiceClassList);
				List<ExtendedFullTimeEventModel> timeEventList =
					FullTimeEventDataAccess.GetAllExtendedFullTimeEventsPerServiceTemplate(plottableService
						.ServiceTemplateId);
				return ExtractDataLineFromTimeEventsPerService(firstPoint, plottableService.DataLine, timeEventList, plottableService.StartTime);
				}

		public DataPoint ExtractDataLineFromTimeEventsPerService(DataPoint firstPoint, List<DataPoint> dataLine, List<ExtendedFullTimeEventModel> timeEventList, int startTime)
			{
			int actualTime = startTime;
			DataPoint lastPoint=null;
			// The firstpoint establishes a connection line from the previous service
			// For the first service, it should be null.
			// The startTime should be the starTime of the service processed right now. firstPoint will set the the proper timing automatically :-)
			if (firstPoint != null)
				{
				dataLine.Add(firstPoint);
				}
			foreach (var fullTimeEvent in timeEventList)
				{
				actualTime += fullTimeEvent.ArrivalTime;
				fullTimeEvent.ArrivalTimeText = TimeConverters.MinutesToString(actualTime);
				DataPoint point = GetFirstDataPoint(fullTimeEvent, actualTime);
				dataLine.Add(point);
				lastPoint = point;
				if (fullTimeEvent.WaitTime > 0)
					{
					actualTime += fullTimeEvent.WaitTime;
					DataPoint point2 = GetSecondDataPoint(actualTime, point.X);
					dataLine.Add(point2);
					lastPoint = point2;
					}
				fullTimeEvent.DepartureTimeText = TimeConverters.MinutesToString(actualTime);
				}
			return lastPoint;
			}
		private DataPoint GetFirstDataPoint(FullTimeEventModel fullTimeEvent, int actualTime)
			{
			DataPoint output = new DataPoint();
			output.Y = actualTime;
			output.X = LocationList.Where(x => x.Id == fullTimeEvent.LocationId)
				.Select(x => x.Order).First();
			return output;
			}

		private DataPoint GetSecondDataPoint(int actualTime, double xValue)
			{
			DataPoint output = new DataPoint();
			output.Y = actualTime;
			output.X = xValue;
			return output;
			}

		public Color GetLineColor(string serviceType, List<ServiceClassModel> serviceClassList)
			{
			var serviceClass = ServiceClassDataAccess.GetServiceClassModelFromString(serviceType, serviceClassList);
			if (serviceClass == null)
				{
				return Color.Magenta;// Ugly default value
				}
			return Color.FromName(serviceClass.Color);
			}

		}
	}
