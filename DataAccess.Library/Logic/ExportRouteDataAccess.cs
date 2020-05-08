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

		public string ExportServiceTable()
			{
			List<ServiceModel> serviceList = ServiceDataAccess.GetServicesPerRoute(RouteId);
			return WriteServiceHeader()+WriteServiceData(serviceList);
			}

		public string WriteServiceHeader()
			{
			return $"\"Services\"{sep}\"Id\"{sep}\"ServiceName\"{sep}\"ServiceAbbreviation\"{sep}\"ServiceDescription\"{sep}" +
								$"\"ServiceType\"{sep}\"ServiceDirectionId\"{sep}\"CalculatedDuration\"{sep}\"RouteId\"\r\n";
			}

		public string WriteServiceData(List<ServiceModel> serviceList)
			{
			var output = "";
			foreach (var item in serviceList)
				{
				output+=  $"\"Service\"{sep}\"{item.Id}\"{sep}\"{item.ServiceName}\"{sep}\"{item.ServiceAbbreviation}\"{sep}\"" +
				          $"{item.ServiceDescription}\"{sep}\"{item.ServiceType}\"{sep}\"{item.ServiceDirectionId}\"{sep}" +
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
				output += $"\"TimeEvent\"{sep}\"{item.Id}\"{sep}\"{item.EventType}\"{sep}\"{item.ArrivalTime}\"{sep}\"{item.WaitTime}\"{sep}\"{item.ServiceId}\"{sep}\"{item.LocationId}\"{sep}\"{item.Order}\"\r\n";
				}
			return output;
			}

		public string ExportServiceInstanceTable()
			{
			List<ServiceInstanceModel> serviceInstanceList =
				ServiceInstanceDataAccess.GetServiceInstancesPerRoute(RouteId);
			return WriteServiceInstanceHeader()+ WriteServiceInstanceData(serviceInstanceList);
			}

		public string WriteServiceInstanceHeader()
			{
			return $"\"ServiceInstances\"{sep}\"Id\"{sep}\"ServiceInstanceName\"{sep}\"ServiceInstanceAbbreviation\"{sep}" +
			       $"\"StartTime\"{sep}\"EndTime\"{sep}\"ServiceId\"\r\n";
			}

		public string WriteServiceInstanceData(List<ServiceInstanceModel> serviceInstanceList)
			{
			var output = "";
			foreach (var item in serviceInstanceList)
				{
				output += $"\"ServiceInstance\"{sep}\"{item.Id}\"{sep}\"{item.ServiceInstanceName}\"{sep}" +
				          $"\"{item.ServiceInstanceAbbreviation}\"{sep}\"{item.StartTime}\"{sep}" +
				          $"\"{item.EndTime}\"{sep}\"{item.ServiceId}\"\r\n";
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
			return $"\"ConnectTtSis\"{sep}\"Id\"{sep}\"TimetableId\"{sep}\"ServiceInstanceId\"\r\n";
			}

		public string WriteConnectData(List<ConnectTtSiModel> connectList)
			{
			var output = "";
			foreach (var item in connectList)
				{
				output += $"\"ConnectTtSi\"{sep}\"{item.Id}\"{sep}\"{item.TimetableId}\"{sep}\"{item.ServiceInstanceId}\"\r\n";
				}
			return output;
			}

		}
	}
