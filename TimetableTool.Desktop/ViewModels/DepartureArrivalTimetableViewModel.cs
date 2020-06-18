using Caliburn.Micro;
using DataAccess.Library.Logic;
using DataAccess.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimetableTool.Desktop.ViewModels
	{
	#region Properties
	public class DepartureArrivalTimetableViewModel: Screen
		{
		public int RouteId { get; set; }
		public int TimetableId { get; set; }
		public List<DepartureArrivalTimetableModel> ServiceList { get; set; }

		private BindableCollection<DepartureArrivalTimetableModel> _filteredServiceList;

		public BindableCollection<DepartureArrivalTimetableModel> FilteredServiceList
			{
			get
				{
				return _filteredServiceList;
				}
			set
				{
				_filteredServiceList = value;
				}
			}

		private BindableCollection<LocationModel> _locationList;

		public BindableCollection<LocationModel> LocationList
			{
			get
				{
				return _locationList;
				}
			set
				{
				_locationList = value;

				}
			}

		private LocationModel _selectedLocation;

		public LocationModel SelectedLocation
			{
			get
				{
				return _selectedLocation;
				}
			set
				{
				_selectedLocation = value;
				ServiceList =
					DepartureArrivalTimetableDataAccess.GetDeparturesAndArrivals(TimetableId,
						SelectedLocation.Id);
				ServiceList = ServiceList.OrderBy(x => x.ArrivalTime).ToList();
				FilteredServiceList= new BindableCollection<DepartureArrivalTimetableModel>(ServiceList);
				NotifyOfPropertyChange(()=>FilteredServiceList);
				}
			}
		#endregion

		#region Initialization

		protected override async void OnViewLoaded(object view)
			{
			base.OnViewLoaded(view);
			LocationList= new BindableCollection<LocationModel>(LocationDataAccess.GetAllLocationsPerRoute(RouteId));
			NotifyOfPropertyChange(()=>LocationList);
			}
		#endregion


		}
	}
