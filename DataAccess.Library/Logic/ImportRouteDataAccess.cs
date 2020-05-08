using DataAccess.Library.Models;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;

namespace DataAccess.Library.Logic
	{
	public class ImportRouteDataAccess
		{
		private string importPath;
		private int newRouteId = -1;
		private Dictionary<int, int> LocationKeys = new Dictionary<int, int>();
		private Dictionary<int,int> ServiceDirectionKeys = new Dictionary<int,int>(); 
		private Dictionary<int,int> ServiceKeys = new Dictionary<int,int>(); 
		private Dictionary<int,int> ServiceInstanceKeys = new Dictionary<int,int>();
		private Dictionary<int,int> TimetableKeys = new Dictionary<int,int>();
		public ImportRouteDataAccess(string path)
			{
			importPath = path;
			ParseFile(importPath);
			}

		// Low level support functions

		private void ParseFile(string input)
			{
			TextFieldParser parser = new TextFieldParser(input);
			parser.HasFieldsEnclosedInQuotes = true;
			parser.SetDelimiters(",");

			string[] fields;

			while (!parser.EndOfData)
				{
				try
					{
				fields = parser.ReadFields(); // reads one line
				if (fields.GetLength(0) > 0)
					{
					ParseLine(fields);
					}
					}
				catch (Exception e)
					{
					Console.WriteLine(e);
					}
				}
			parser.Close();
			}

		private string Unquote(string input)
			{
			return input.Substring(1, input.Length - 2);
			}

		private void ParseLine(string[] fields)
			{
			switch (fields[0])
				{
					case "Routes":
						{
						break;
						}
					case "Route":
						{
						ImportRoute(fields);
						break;
						}
					case "Locations":
						{
						break;
						}
					case "Location":
						{
						ImportLocations(fields);
						break;
						}
					case "ServiceDirections":
						{
						break;
						}
					case "ServiceDirection":
						{
						ImportServiceDirection(fields);
						break;
						}
					case "Services":
						{
						break;
						}
					case "Service":
						{
						ImportServices(fields);
						break;
						}
					case "TimeEvents":
						{
						break;
						}
					case "TimeEvent":
						{
						ImportTimeEvents(fields);
						break;
						}
					case "ServiceInstances":
						{
						break;
						}
					case "ServiceInstance":
						{
						ImportServiceInstances(fields);
						break;
						}
					case "Timetables":
						{
						break;
						}
					case "Timetable":
						{
						ImportTimetable(fields);
						break;
						}
					case "ConnectTtSis":
						{
						break;
						}
					case "ConnectTtSi":
						{
						ImportConnectTiSi(fields);
						break;
						}
				}
			}

		private void ImportConnectTiSi(string[] fields)
			{
			var connect= new ConnectTtSiModel();
			connect.Id= int.Parse(fields[1]);
			var oldTimetableId= int.Parse(fields[2]);
			connect.TimetableId = TimetableKeys.GetValueOrDefault(oldTimetableId, 0);
			var oldServiceInstanceId = int.Parse(fields[3]);
			connect.ServiceInstanceId = ServiceInstanceKeys.GetValueOrDefault(oldServiceInstanceId, 0);
			ConnectTtSiDataAccess.InsertConnection(connect.ServiceInstanceId,connect.TimetableId);
			}

		private void ImportTimetable(string[] fields)
			{
			//return $"\"Timetables\"{sep}\"Id\"{sep}\"TimetableName\"{sep}\"TimetableAbbreviation\"{sep}" +
			 //      $"\"TimetableDescription\"{sep}\"IsMultiDirection\"{sep}\"ServiceDirectionId\"{sep}\"RouteId\"\r\n";
			var timetable= new TimetableModel();
			timetable.Id= int.Parse(fields[1]);
			timetable.TimetableName = fields[2];
			timetable.TimetableAbbreviation = fields[3];
			timetable.TimetableDescription = fields[4];
			timetable.IsMultiDirection = bool.Parse(fields[5]);
			var oldServiceDirectionId = int.Parse(fields[6]);
			timetable.ServiceDirectionId =
				ServiceDirectionKeys.GetValueOrDefault(oldServiceDirectionId, 0);
			timetable.RouteId = newRouteId;
			var newTimetableId = TimetableDataAccess.InsertTimetableForRoute(timetable);
			TimetableKeys.Add(timetable.Id,newTimetableId);
			}

		private void ImportServiceInstances(string[] fields)
			{
			var serviceInstance= new ServiceInstanceModel();
			serviceInstance.Id= int.Parse(fields[1]);
			serviceInstance.ServiceInstanceName = fields[2];
			serviceInstance.ServiceInstanceAbbreviation = fields[3];
			serviceInstance.StartTime= int.Parse(fields[4]);
			serviceInstance.EndTime = int.Parse(fields[5]);
			var oldServiceId=int.Parse(fields[6]);
			serviceInstance.ServiceId = ServiceKeys.GetValueOrDefault(oldServiceId, 0);
			var newServiceInstanceId = ServiceInstanceDataAccess.InsertServiceInstance(serviceInstance);
			ServiceInstanceKeys.Add(serviceInstance.Id,newServiceInstanceId);
			}

		private void ImportTimeEvents(string[] fields)
			{
			var timeEvent = new TimeEventModel();
			timeEvent.Id= int.Parse(fields[1]);
			timeEvent.EventType = fields[2];
			timeEvent.ArrivalTime=int.Parse(fields[3]);
			timeEvent.WaitTime=int.Parse(fields[4]);
			var oldServiceId= int.Parse(fields[5]);
			timeEvent.ServiceId = ServiceKeys.GetValueOrDefault(oldServiceId, 0);
			var oldLocationid= int.Parse(fields[6]);
			timeEvent.LocationId = LocationKeys.GetValueOrDefault(oldLocationid, 0);
			timeEvent.Order= int.Parse(fields[7]);
			TimeEventDataAccess.InsertTimeEventForService(timeEvent);
			}

		private void ImportServices(string[] fields)
			{
			var service = new ServiceModel();
			service.Id = int.Parse(fields[1]);
			service.ServiceName = fields[2];
			service.ServiceAbbreviation = fields[3];
			service.ServiceDescription = fields[4];
			service.ServiceType = fields[5];
			int oldServiceDirectionId = int.Parse(fields[6]);
			service.ServiceDirectionId = ServiceDirectionKeys.GetValueOrDefault(oldServiceDirectionId, 0);
			service.CalculatedDuration= int.Parse(fields[7]);
			service.RouteId = newRouteId;
			var newServiceId = ServiceDataAccess.InsertServiceForRoute(service);
			ServiceKeys.Add(service.Id,newServiceId);
			}

		private void ImportServiceDirection(string[] fields)
			{
			var serviceDirection= new ServiceDirectionModel();
			serviceDirection.Id=int.Parse(fields[1]);
			serviceDirection.ServiceDirectionName = fields[2];
			serviceDirection.ServiceDirectionAbbreviation = fields[3];
			serviceDirection.IsDescending = bool.Parse(fields[5]);
			serviceDirection.RouteId = newRouteId;
			var newServiceDirectionId = ServiceDirectionDataAccess.InsertServiceDirection(serviceDirection);
			ServiceDirectionKeys.Add(serviceDirection.Id,newServiceDirectionId);
			}

		private void ImportLocations(string[] fields)
			{
			var location= new LocationModel();
			location.Id=int.Parse(fields[1]);
			location.LocationName = fields[2];
			location.LocationAbbreviation = fields[3];
			location.NumberOfTracks= int.Parse(fields[4]);
			location.Order=int.Parse(fields[5]);
			location.RouteId = newRouteId;
			var newLocationId = LocationDataAccess.InsertLocationForRoute(location);
			LocationKeys.Add(location.Id, newLocationId);
			}

		private void ImportRoute(string[] fields)
			{
			var route = new RouteModel();
			route.Id = int.Parse(fields[1]);
			route.RouteName = fields[2];
			route.RouteAbbreviation=fields[3];
			route.RouteDescription = fields[4];
			newRouteId = RouteDataAccess.InsertRoute(route);
			}


		}
	}
