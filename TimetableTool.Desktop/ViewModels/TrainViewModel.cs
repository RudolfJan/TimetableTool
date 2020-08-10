using Caliburn.Micro;
using DataAccess.Library.Logic;
using DataAccess.Library.Models;
using System;
using TimetableTool.Desktop.EventModels;

namespace TimetableTool.Desktop.ViewModels
	{
	public class TrainViewModel : Screen
		{
		public RouteModel Route { get; set; }
		private readonly IEventAggregator _events;
		private BindableCollection<TrainModel> _trainList;

		public BindableCollection<TrainModel> TrainList
			{
			get
				{
				return _trainList;
				}
			set
				{
				_trainList = value;
				NotifyOfPropertyChange(() => TrainList);
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

				TrainSelectedEvent trainSelectedEvent = new TrainSelectedEvent();
				trainSelectedEvent.SelectedTrain = _selectedTrain;
				_events.PublishOnUIThreadAsync(trainSelectedEvent);
				NotifyOfPropertyChange(() => CanEdit);
				NotifyOfPropertyChange(() => CanDelete);
				}
			}

		public int TrainId { get; set; }

		private string _trainName;
		public string TrainName
			{
			get
				{
				return _trainName;
				}
			set
				{
				_trainName = value;
				NotifyOfPropertyChange(()=>CanSave);
				NotifyOfPropertyChange(()=>TrainName);
				}
			}

		private string _trainAbbreviation;
		public string TrainAbbreviation
			{
			get
				{
				return _trainAbbreviation;
				}
			set
				{
				_trainAbbreviation = value;
				NotifyOfPropertyChange(() => CanSave);
				NotifyOfPropertyChange(() => TrainAbbreviation);
				}
			}

		private string _trainDescription;
		public string TrainDescription
			{
			get
				{
				return _trainDescription;
				}
			set
				{
				_trainDescription = value;
				NotifyOfPropertyChange(() => CanSave);
				NotifyOfPropertyChange(() => TrainDescription);
				}
			}

		private string _trainClass;
		public string TrainClass
			{
			get
				{
				return _trainClass;
				}
			set
				{
				_trainClass = value;
				NotifyOfPropertyChange(() => CanSave);
				NotifyOfPropertyChange(() => TrainClass);
				}
			}

		public TrainViewModel(IEventAggregator events)
			{
			_events = events;
			}

		protected override void OnViewLoaded(object view)
			{
			base.OnViewLoaded(view);
			TrainList = new BindableCollection<TrainModel>(TrainDataAccess.GetAllTrainsPerRoute(Route.Id));
			}

		public bool CanEdit
			{
			get
				{
				return SelectedTrain != null;
				}
			}

		public bool CanDelete
			{
			get
				{
				return SelectedTrain != null;
				}
			}

		public void Edit()
			{
			TrainName = SelectedTrain.TrainName;
			TrainAbbreviation = SelectedTrain.TrainAbbreviation;
			TrainDescription = SelectedTrain.TrainDescription;
			TrainClass = SelectedTrain.TrainClass;
			TrainId = SelectedTrain.Id;
			}

		public void Delete()
			{
			TrainDataAccess.DeleteTrain(SelectedTrain.Id);
			TrainList.Remove(SelectedTrain);
			SelectedTrain = null;
			NotifyOfPropertyChange(() => TrainList);
			}

		public bool CanSave
			{
			get
				{
				return TrainName?.Length > 0 &&
							 TrainAbbreviation?.Length > 0 &&
							 TrainClass?.Length > 0;
				}
			}

		public void Save()
			{
			
			if (TrainId == 0)
				{
				var newTrain = new TrainModel();
				newTrain.TrainName = TrainName;
				newTrain.TrainAbbreviation = TrainAbbreviation;
				newTrain.TrainDescription = TrainDescription;
				newTrain.TrainClass = TrainClass;
				newTrain.RouteId = Route.Id;
				newTrain.Id = TrainDataAccess.InsertTrain(newTrain);
				TrainList.Add(newTrain);
				}
			else
				{
				SelectedTrain.TrainName = TrainName;
				SelectedTrain.TrainAbbreviation = TrainAbbreviation;
				SelectedTrain.TrainDescription = TrainDescription;
				SelectedTrain.TrainClass = TrainClass;
				TrainDataAccess.UpdateTrain(SelectedTrain);
				}
			Clear();
			}

		public void Clear()
			{
			TrainName = string.Empty;
			TrainAbbreviation = string.Empty;
			TrainDescription= String.Empty;
			TrainClass = string.Empty;
			TrainId = 0;
			NotifyOfPropertyChange(() => TrainList);
			NotifyOfPropertyChange(() => SelectedTrain);
			}

		}

	}

