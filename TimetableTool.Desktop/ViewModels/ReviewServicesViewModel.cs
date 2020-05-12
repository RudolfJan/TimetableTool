using Caliburn.Micro;
using DataAccess.Library.Logic;
using DataAccess.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TimetableTool.Desktop.ViewModels
	{
	public class ReviewServicesViewModel: Screen
		{
		private BindableCollection<ServiceModel> _createdServices= new BindableCollection<ServiceModel>();

		public BindableCollection<ServiceModel> CreatedServices

			{
			get { return _createdServices; }
			set
				{
				_createdServices = value;
				NotifyOfPropertyChange(()=>CreatedServices);
				}
			}

		public bool IsSaved { get; set; }
		public async Task OK()
			{
			foreach (var item in CreatedServices)
				{
				ServicesDataAccess.InsertService(item);
				}

			IsSaved = true;
			CreatedServices.Clear();
			await TryCloseAsync();
			}

		public async Task Cancel()
			{
			await TryCloseAsync();
			}
		}
	}
