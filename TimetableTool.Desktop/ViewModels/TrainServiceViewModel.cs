using Caliburn.Micro;
using DataAccess.Library.Logic;
using DataAccess.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimetableTool.Desktop.ViewModels
	{
	public class TrainServiceViewModel: Screen
		{
		public TrainModel Train { get; set; }
		private int lastLocationInTrainService = 0;
		private int currentServiceEndTime = 0;

		public List<ExtendedServiceModel> ServiceSourceList { get; set; }


		private BindableCollection<ExtendedServiceModel> _filteredServiceSourceList;

		public BindableCollection<ExtendedServiceModel> FilteredServiceSourceList
			{
			get
				{
				return _filteredServiceSourceList;
				}
			set
				{
				_filteredServiceSourceList = value;
				NotifyOfPropertyChange(() => FilteredServiceSourceList);
				}
			}







		private ExtendedServiceModel _selectedServiceSource;

		public ExtendedServiceModel SelectedServiceSource
			{
			get
				{
				return _selectedServiceSource;
				}
			set
				{
				_selectedServiceSource = value;
				NotifyOfPropertyChange(()=>SelectedServiceSource);
				NotifyOfPropertyChange(()=>CanAddToTrainService);
				}
			}


		private BindableCollection<ExtendedServiceModel> _servicesInTrainServiceList;

		public BindableCollection<ExtendedServiceModel> ServicesInTrainServiceList
			{
			get
				{
				return _servicesInTrainServiceList;
				}
			set
				{
				_servicesInTrainServiceList = value;
				NotifyOfPropertyChange(()=>ServicesInTrainServiceList);
				}
			}

		private ExtendedServiceModel _selectedServiceInTrainService;
		public ExtendedServiceModel SelectedServiceInTrainService
			{
			get
				{
				return _selectedServiceInTrainService;
				}
			set
				{
				_selectedServiceInTrainService = value;
				NotifyOfPropertyChange(()=>SelectedServiceInTrainService);
				NotifyOfPropertyChange(()=>CanRemoveFromTrainService);
				}
			}


		protected override void OnViewLoaded(object view)
			{
			base.OnViewLoaded(view);
			ServiceSourceList =
				ServicesDataAccess.GetServicesPerRouteButNotInTrainServices(Train.RouteId);
			ServicesInTrainServiceList = new BindableCollection<ExtendedServiceModel>(
				ServicesDataAccess.GetServicesPerRoutePerTrainInTrainServices(Train.RouteId, Train.Id));
			InitFilteredServiceSourceList();
			}

		private void InitFilteredServiceSourceList()
			{
			if (ServicesInTrainServiceList.Count > 0)
				{
				lastLocationInTrainService =
					ServicesInTrainServiceList.Last()
						.EndLocationId; // This works because the list is supposed to be sorted always.
				currentServiceEndTime = ServicesInTrainServiceList.Last().EndTime;
				FilteredServiceSourceList = new BindableCollection<ExtendedServiceModel>(ServiceSourceList
					.Where(x => x?.StartLocationId == lastLocationInTrainService)
					.Where(y => y?.StartTime > currentServiceEndTime));
				}
			else
				{
				FilteredServiceSourceList = new BindableCollection<ExtendedServiceModel>(ServiceSourceList);
				}
			}

		public bool CanAddToTrainService
			{
			get
				{
				return SelectedServiceSource != null;
				}
			}

		public bool CanRemoveFromTrainService
			{
			get
				{
				return SelectedServiceInTrainService != null;
				}
			}

		public void AddToTrainService()
			{
			var trainService= new TrainServiceModel();
			trainService.TrainId = Train.Id;
			trainService.ServiceId = SelectedServiceSource.Id;
			TrainServiceDataAccess.InsertTrainService(trainService);
			ServicesInTrainServiceList.Add(SelectedServiceSource);
			ServiceSourceList.Remove(SelectedServiceSource);
			lastLocationInTrainService = SelectedServiceSource.EndLocationId;
			currentServiceEndTime = SelectedServiceSource.EndTime;
			FilteredServiceSourceList = new BindableCollection<ExtendedServiceModel>(ServiceSourceList
				.Where(x => x?.StartLocationId == lastLocationInTrainService)
				.Where(y => y?.StartTime > currentServiceEndTime));
			SelectedServiceSource = null;
			NotifyOfPropertyChange(() => CanAddToTrainService);
			NotifyOfPropertyChange(()=>FilteredServiceSourceList);
			NotifyOfPropertyChange(()=>ServicesInTrainServiceList);
			}

		public void RemoveFromTrainService()
			{
			var trainService = new TrainServiceModel();
			trainService.TrainId = Train.Id;
			trainService.ServiceId = SelectedServiceInTrainService.Id;
			ServiceSourceList.Add(SelectedServiceInTrainService);
			ServicesInTrainServiceList.Remove(SelectedServiceInTrainService);
			TrainServiceDataAccess.DeleteTrainService(trainService);
			InitFilteredServiceSourceList();
			SelectedServiceInTrainService = null;
			NotifyOfPropertyChange(() => CanRemoveFromTrainService);
			NotifyOfPropertyChange(() => ServiceSourceList);
			NotifyOfPropertyChange(() => ServicesInTrainServiceList);
			}
		}
	}
