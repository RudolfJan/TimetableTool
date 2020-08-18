using DataAccess.Library.Logic;
using DataAccess.Library.Models;
using Styles.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TimetableTool.Desktop.ViewModels
	{
	public class StatisticsGraph: Notifier
		{
		private ObservableCollection<ServiceTemplateStatisticsModel> _serviceTemplateStatisticsList;

		public ObservableCollection<ServiceTemplateStatisticsModel> ServiceTemplateStatisticsList
			{
			get
				{
				return _serviceTemplateStatisticsList;
				}
			set
				{
				_serviceTemplateStatisticsList = value;
				OnPropertyChanged("ServiceTemplateStatisticsList");
				}
			}

		private ObservableCollection<RouteStatisticsModel> _routeStatisticsList;

		public ObservableCollection<RouteStatisticsModel> RouteStatisticsList
			{
			get
				{
				return _routeStatisticsList;
				}
			set
				{
				_routeStatisticsList = value;
				OnPropertyChanged("RouteStatisticsList");
				}
			}



		public StatisticsGraph()
			{
			ServiceTemplateStatisticsList= new ObservableCollection<ServiceTemplateStatisticsModel>(ServiceTemplateDataAccess.GetServiceTemplateStatistics().OrderBy(x=>x.RouteAbbreviation));
			RouteStatisticsList= new ObservableCollection<RouteStatisticsModel>(RouteDataAccess.GetRouteStatistics().OrderBy(x => x.RouteAbbreviation));
			}
		}
	}
