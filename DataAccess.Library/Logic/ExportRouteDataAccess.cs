using DataAccess.Library.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace DataAccess.Library.Logic
	{
	public class ExportRouteDataAccess
		{
		public int RouteId { get; set; }
		public string sep = ",";

		public ExportRouteDataAccess(int routeId)
			{
			RouteId = routeId;
			}

		public string ExportRouteTable()
			{
			RouteModel route = RouteDataAccess.GetRouteById(RouteId);
			var output=WriteRouteHeader() + WriteRouteData(route);
			return output;
			}

		public string WriteRouteHeader()
			{
			return $"\"Routes\"{sep}\"Id\"{sep}\"RouteName\"{sep}\"RouteAbbreviation\"{sep}\"RouteDescription\"\r\n";
			}

		public string WriteRouteData(RouteModel route)
			{
			return $"\"Route\"{sep}\"{route.Id}\"{sep}\"{route.RouteName}\"{sep}\"{route.RouteAbbreviation}\"{sep}\"{route.RouteDescription}\"\r\n";
			}

		public string ExportLocationsTable()
			{
			List<LocationModel> locationList = LocationDataAccess.GetAllLocationsPerRoute(RouteId);
			return WriteLocationHeader()+WriteLocationData(locationList);
			}

		// public string WriteLocationHeader
		public string WriteLocationHeader()
			{
			return $"\"Locations\"{sep}\"Id\"{sep}\"LocationName\"{sep}\"LocationAbbreviation\"{sep}\"NumberOfTracks\"{sep}\"Order\"{sep}\"RouteId\"\r\n";
			}

		public string WriteLocationData(List<LocationModel> locationList)
			{
			var output = "";
			foreach (var item in locationList)
				{
				output+=  $"\"Location\"{sep}\"{item.Id}\"{sep}\"{item.LocationName}\"{sep}\"{item.LocationAbbreviation}\"{sep}\"{item.NumberOfTracks}\"{sep}\"{item.Order}\"{sep}\"{item.RouteId}\"\r\n";
				}
			return output;
			}

		public string ExportServiceDirectionTable()
			{
			List<ServiceDirectionModel> serviceDirections =
				ServiceDirectionDataAccess.GetAllServiceDirectionsPerRoute(RouteId);
			return WriteServiceDirectionHeader()+ WriteServiceDirectionData(serviceDirections);
			}

		public string WriteServiceDirectionHeader()
			{
			return $"\"ServiceDirections\"{sep}\"Id\"{sep}\"ServiceDirectionName\"{sep}\"ServiceDirectionAbbreviation\"{sep}\"RouteId\"{sep}\"IsDescending\"\r\n";
			}

		public string WriteServiceDirectionData(List<ServiceDirectionModel> serviceDirections)
			{
			var output = "";
			foreach (var item in serviceDirections)
				{
				output += $"\"ServiceDirection\"{sep}\"{item.Id}\"{sep}\"{item.ServiceDirectionName}\"{sep}\"{item.ServiceDirectionAbbreviation}\"{sep}\"{item.RouteId}\"{sep}\"{item.IsDescending}\"\r\n";
				}
			return output;
			}

		public string ExportServiceTemplateTable()
			{
			List<ServiceTemplateModel> serviceTemplateList = ServiceTemplateDataAccess.GetServiceTemplatesPerRoute(RouteId);
			return WriteServiceTemplateHeader()+WriteServiceTemplateData(serviceTemplateList);
			}

		public string WriteServiceTemplateHeader()
			{
			return $"\"ServiceTemplates\"{sep}\"Id\"{sep}\"ServiceTemplateName\"{sep}\"ServiceTemplateAbbreviation\"{sep}\"ServiceTemplateDescription\"{sep}" +
								$"\"ServiceType\"{sep}\"ServiceDirectionId\"{sep}\"CalculatedDuration\"{sep}\"RouteId\"\r\n";
			}

		public string WriteServiceTemplateData(List<ServiceTemplateModel> serviceList)
			{
			var output = "";
			foreach (var item in serviceList)
				{
				output+=  $"\"ServiceTemplate\"{sep}\"{item.Id}\"{sep}\"{item.ServiceTemplateName}\"{sep}\"{item.ServiceTemplateAbbreviation}\"{sep}\"" +
				          $"{item.ServiceTemplateDescription}\"{sep}\"{item.ServiceType}\"{sep}\"{item.ServiceDirectionId}\"{sep}" +
				          $"\"{item.CalculatedDuration}\"{sep}\"{item.RouteId}\"\r\n";
				}
			return output;
			}

		public string ExportTimeEventsTable()
			{
			List<TimeEventModel> timeEventList = TimeEventDataAccess.GetAllTimeEventsPerRoute(RouteId);
			return WriteTimeEventHeader()+WriteTimeEventData(timeEventList);
			}

		public string WriteTimeEventHeader()
			{
			return $"\"TimeEvents\"{sep}\"Id\"{sep}\"EventType\"{sep}\"ArrivalTime\"{sep}\"WaitTime\"{sep}\"ServiceId\"{sep}\"LocationId\"{sep}\"Order\"\r\n";
			}

		public string WriteTimeEventData(List<TimeEventModel> timeEventList)
			{
			string output = "";
			foreach (var item in timeEventList)
				{
				output += $"\"TimeEvent\"{sep}\"{item.Id}\"{sep}\"{item.EventType}\"{sep}\"{item.ArrivalTime}\"{sep}\"{item.WaitTime}\"{sep}\"{item.ServiceTemplateId}\"{sep}\"{item.LocationId}\"{sep}\"{item.Order}\"\r\n";
				}
			return output;
			}

		public string ExportServiceTable()
			{
			List<ServiceModel> serviceList =
				ServicesDataAccess.GetServicesPerRoute(RouteId);
			return WriteServiceHeader()+ WriteServiceData(serviceList);
			}

		public string WriteServiceHeader()
			{
			return $"\"Services\"{sep}\"Id\"{sep}\"ServiceName\"{sep}\"ServiceAbbreviation\"{sep}" +
			       $"\"StartTime\"{sep}\"EndTime\"{sep}\"ServiceTemplateId\"\r\n";
			}

		public string WriteServiceData(List<ServiceModel> serviceList)
			{
			var output = "";
			foreach (var item in serviceList)
				{
				output += $"\"Service\"{sep}\"{item.Id}\"{sep}\"{item.ServiceName}\"{sep}" +
				          $"\"{item.ServiceAbbreviation}\"{sep}\"{item.StartTime}\"{sep}" +
				          $"\"{item.EndTime}\"{sep}\"{item.ServiceTemplateId}\"\r\n";
				}
			return output;
			}

		public string ExportTimetableTable()
			{
			List<TimetableModel> timetableList = TimetableDataAccess.GetAllTimetablesPerRoute(RouteId);
			return WriteTimetableHeader()+WriteTimetableData(timetableList);
			}

		public string WriteTimetableHeader()
			{
			return $"\"Timetables\"{sep}\"Id\"{sep}\"TimetableName\"{sep}\"TimetableAbbreviation\"{sep}" +
			       $"\"TimetableDescription\"{sep}\"IsMultiDirection\"{sep}\"ServiceDirectionId\"{sep}\"RouteId\"\r\n"; 
			}

		public string WriteTimetableData(List<TimetableModel> timetableList)
			{
			var output = "";
			foreach (var item in timetableList)
				{
				output += $"\"Timetable\"{sep}\"{item.Id}\"{sep}\"{item.TimetableName}\"{sep}\"{item.TimetableAbbreviation}\"{sep}" +
				          $"\"{item.TimetableDescription}\"{sep}\"{item.IsMultiDirection}\"{sep}" +
				          $"\"{item.ServiceDirectionId}\"{sep}\"{item.RouteId}\"\r\n"; 
				}
			return output;
			}

		public string ExportConnectTiSi()
			{
			List<ConnectTtSiModel> connectList = ConnectTtSiDataAccess.GetAllConnectTtSiPerRoute(RouteId);
			return WriteConnectHeader()+WriteConnectData(connectList);
			}

		public string WriteConnectHeader()
			{
			return $"\"ConnectTtSis\"{sep}\"Id\"{sep}\"TimetableId\"{sep}\"ServiceId\"\r\n";
			}

		public string WriteConnectData(List<ConnectTtSiModel> connectList)
			{
			var output = "";
			foreach (var item in connectList)
				{
				output += $"\"ConnectTtSi\"{sep}\"{item.Id}\"{sep}\"{item.TimetableId}\"{sep}\"{item.ServiceId}\"\r\n";
				}
			return output;
			}

		public string ExportTrains()
			{
			List<TrainModel> trainList = TrainDataAccess.GetAllTrainsPerRoute(RouteId);
			return WriteTrainHeader() + WriteTrainData(trainList);
			}

		public string WriteTrainHeader()
			{
			return $"\"Trains\"{sep}\"Id\"{sep}\"TrainName\"{sep}\"TrainAbbreviation\"{sep}\"TrainDescription\"{sep}\"TrainClass\"{sep}\"RouteId\"\r\n";
			}

		public string WriteTrainData(List<TrainModel> trainList)
			{
			var output = "";
			foreach (var item in trainList)
				{
				output += $"\"Train\"{sep}\"{item.Id}\"{sep}\"{item.TrainName}\"{sep}\"{item.TrainAbbreviation}\"{sep}\"{item.TrainDescription}\"{sep}\"{item.TrainClass}\"{sep}\"{item.RouteId}\"\r\n";
				}
			return output;
			}

		public string ExportTrainServices()
			{
			List<TrainServiceModel> trainServiceList = TrainServiceDataAccess.GetAllTrainServicesPerRoute(RouteId);
			return WriteTrainHeader() + WriteTrainServiceData(trainServiceList);
			}

		public string WriteTrainServiceHeader()
			{
			return $"\"TrainServices\"{sep}\"Id\"{sep}\"ServiceId\"{sep}\"TrainId\"\r\n";
			}

		public string WriteTrainServiceData(List<TrainServiceModel> trainServiceList)
			{
			var output = "";
			foreach (var item in trainServiceList)
				{
				output += $"\"TrainService\"{sep}\"{item.Id}\"{sep}\"{item.ServiceId}\"{sep}\"{item.TrainId}\"\r\n";
				}
			return output;
			}
		}
	}
