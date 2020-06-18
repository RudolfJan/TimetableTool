using DataAccess.Library.Models;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Library.Logic
	{
	public class DepartureArrivalTimetableDataAccess
		{
		public static List<DepartureArrivalTimetableModel> GetDeparturesAndArrivals(int timetableId,
			int locationId)
			{
			List<DepartureArrivalTimetableModel> output= new List<DepartureArrivalTimetableModel>();
			var serviceList = ServicesDataAccess.GetServicesPerTimetable(timetableId);
			foreach (var service in serviceList)
				{
				var departureArrival= new DepartureArrivalTimetableModel();
				departureArrival.ServiceName = service.ServiceName;
				departureArrival.ServiceAbbrev = service.ServiceAbbreviation;
				departureArrival.ServiceId = service.Id;
				departureArrival.StartTime = service.StartTime;
				departureArrival.EndTime = service.EndTime;
				var found = false;
				var serviceDirection = ServiceDirectionDataAccess.GetServiceDirectionByServiceTemplateId(service.ServiceTemplateId);
				var timeEventList =
					FullTimeEventDataAccess.GetAllFullTimeEventsPerServiceTemplate(service.ServiceTemplateId);

				if (serviceDirection.IsDescending)
					{
					timeEventList = timeEventList.OrderByDescending(x => x.LocationOrder).ToList();
					}
				else
					{
					timeEventList = timeEventList.OrderBy(x => x.LocationOrder).ToList();
					}

				int arrivalTime = departureArrival.StartTime;
				int departureTime = departureArrival.StartTime;
				foreach (var timeEvent in timeEventList)
					{
					departureArrival.ArrivalTime += timeEvent.ArrivalTime;
					departureArrival.DepartureTime += timeEvent.ArrivalTime + timeEvent.WaitTime;
					if (timeEvent.LocationId == locationId)
						{
						found = true; // need to filter, if the location is not included in the service, do not show it.
						departureArrival.LocationName = timeEvent.LocationName;
						departureArrival.LocationAbbrev = timeEvent.LocationAbbreviation;
						departureArrival.EventType = timeEvent.EventType;
						departureArrival.ArrivalTime = arrivalTime;
						departureArrival.DepartureTime = departureTime;
						}

					if (timeEvent.EventType == "S")
						{
						departureArrival.OriginName = timeEvent.LocationName;
						departureArrival.OriginAbbrev = timeEvent.LocationAbbreviation;
						}

					if (timeEvent.EventType == "E")
						{
						departureArrival.DestinationName = timeEvent.LocationName;
						departureArrival.DestinationAbbrev = timeEvent.LocationAbbreviation;
						}


					}

				if (found)
					{
					output.Add(departureArrival);
					}
				}
			return output;
			}
		}
	}
