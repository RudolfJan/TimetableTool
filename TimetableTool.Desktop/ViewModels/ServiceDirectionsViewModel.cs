using Caliburn.Micro;
using DataAccess.Library.Logic;
using DataAccess.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace TimetableTool.Desktop.ViewModels
	{
	#region Properties
	public class ServiceDirectionsViewModel : Screen
		{
		public int RouteId { get; set; } = -1;
		public int ServiceDirectionId { get; set; } = -1;
		public bool IsDescending
			{
			get
				{
				return _isDescending;
				}

			set
				{
				_isDescending = value;
				NotifyOfPropertyChange(()=>IsDescending);
				}
			}

		private BindableCollection<ServiceDirectionModel> _serviceDirectionsList;

		public BindableCollection<ServiceDirectionModel> ServiceDirectionsList
			{
			get { return _serviceDirectionsList; }
			set
				{
				_serviceDirectionsList = value;
				NotifyOfPropertyChange(() => ServiceDirectionsList);
				}
			}

		private ServiceDirectionModel _selectedServiceDirection;

		public ServiceDirectionModel SelectedServiceDirection
			{
			get { return _selectedServiceDirection; }
			set
				{
				_selectedServiceDirection = value;
				NotifyOfPropertyChange(() => CanEditServiceDirection);
				}
			}

		private string _ServiceDirectionName;

		public string ServiceDirectionName
			{
			get { return _ServiceDirectionName; }
			set
				{
				_ServiceDirectionName = value;
				NotifyOfPropertyChange(() => ServiceDirectionName);
				NotifyOfPropertyChange(() => CanSaveServicesDirection);
				}
			}

		private string _serviceDirectionAbbreviation;
		private bool isDescending;
		private bool _isDescending;

		public string ServiceDirectionAbbreviation
			{
			get { return _serviceDirectionAbbreviation; }
			set
				{
				_serviceDirectionAbbreviation = value;
				NotifyOfPropertyChange(() => ServiceDirectionAbbreviation);
				NotifyOfPropertyChange(() => CanSaveServicesDirection);
				}
			}


		#endregion

		#region Initialize
		protected override async void OnViewLoaded(object view)
			{
			base.OnViewLoaded(view);
			ServiceDirectionsList = new BindableCollection<ServiceDirectionModel>(ServiceDirectionDataAccess.GetAllServiceDirectionsPerRoute(RouteId));
			}
		#endregion


		public bool CanEditServiceDirection
			{
			get
				{
				return SelectedServiceDirection != null && ServiceDirectionId <= 0;
				}
			}

		public void EditServiceDirection()
			{
			ServiceDirectionAbbreviation = SelectedServiceDirection.ServiceDirectionAbbreviation;
			ServiceDirectionName = SelectedServiceDirection.ServiceDirectionName;
			ServiceDirectionId = SelectedServiceDirection.Id;
			IsDescending = SelectedServiceDirection.IsDescending;
			NotifyOfPropertyChange(() => CanEditServiceDirection);
			NotifyOfPropertyChange(() => CanSaveServicesDirection);
			}

		public bool CanDeleteServiceDirection
			{
			get
				{
				return false;
				}
			}

		public void DeleteServiceDirection()
			{
			// TODO implement this method
			}

		public bool CanSaveServicesDirection
			{
			get
				{
				return ServiceDirectionAbbreviation.Length > 0
					&&
					ServiceDirectionName.Length > 0;
				}
			}

		public void SaveServiceDirection()
			{
			ServiceDirectionModel newServicesDirection = new ServiceDirectionModel();
			newServicesDirection.ServiceDirectionName = ServiceDirectionName;
			newServicesDirection.ServiceDirectionAbbreviation = ServiceDirectionAbbreviation;
			newServicesDirection.RouteId = RouteId;
			newServicesDirection.IsDescending = IsDescending;
			if (ServiceDirectionId > 0)
				{
				newServicesDirection.Id = ServiceDirectionId;
				ServiceDirectionDataAccess.UpdateServiceDirectionForRoute(newServicesDirection);
				}
			else
				{
				ServiceDirectionDataAccess.InsertServiceDirection(newServicesDirection);
				}
			ClearServiceDirection();
			ServiceDirectionsList = new BindableCollection<ServiceDirectionModel>(ServiceDirectionDataAccess.GetAllServiceDirectionsPerRoute(RouteId));
			NotifyOfPropertyChange(() => ServiceDirectionsList);
			}

		public void ClearServiceDirection()
			{
			ServiceDirectionName = "";
			ServiceDirectionAbbreviation = "";
			ServiceDirectionId = 0;
			IsDescending = false;
			NotifyOfPropertyChange(() => CanEditServiceDirection);
			NotifyOfPropertyChange(() => CanSaveServicesDirection);
			}
		}
	}
