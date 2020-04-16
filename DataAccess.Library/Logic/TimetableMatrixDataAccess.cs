using DataAccess.Library.Models;
using SQLiteDataAccess.Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using TimetableTool.DataAccessLibrary.Logic;

namespace DataAccess.Library.Logic
	{
	public class TimetableMatrixDataAccess
		{
		public static TimetableMatrixModel ReadTimetableMatrix(int timetableId)
			{
			// TODO wrap this all in a transaction, for better performance
			TimetableMatrixModel matrixModel = new TimetableMatrixModel();

			// Get TimetableMode to retrieve name

			var timetable = TimetableDataAccess.GetTimetableById(timetableId);
			matrixModel.TimetableName = timetable.TimetableName;
			matrixModel.TimetableId = timetable.Id;
			matrixModel.RouteId = timetable.RouteId;

			// Now get a view, representing ServiceInstances

			List<ServiceInstanceModel> serviceInstanceList = ServiceInstanceDataAccess.GetServiceInstancesPerTimetable(timetable.Id);
			serviceInstanceList = serviceInstanceList.OrderBy(x => x.StartTime).ToList();

			var locationsList = LocationDataAccess.GetAllLocationsPerRoute(matrixModel.RouteId);
			locationsList = locationsList.OrderBy(x => x.Order).ToList();
			var locationsCount= locationsList.Count;

			matrixModel.Matrix = new string[locationsList.Count+1][];
			var columncount = serviceInstanceList.Count + 1;
			string[] columnheaders = new string[columncount];
			columnheaders[0]="---";
			for (int i=0;i< columncount-1; i++)
				{
				columnheaders[i+1]= serviceInstanceList[i].ServiceInstanceAbbreviation;
				}
			matrixModel.Matrix[0]=columnheaders;
			for(int i=0; i<locationsCount;i++)
				{
				string[] row= new string[columncount];
				row[0]= locationsList[i].LocationName;
				matrixModel.Matrix[i+1]= row;
				}

			for (int index=0;index<serviceInstanceList.Count;index++)
				{
				int actualTime= serviceInstanceList[index].StartTime;
				var Timing = GetServiceInstanceTiming(serviceInstanceList[index].Id, locationsList);
				int j=0;
				for(int i=0;i<locationsCount;)
					{
					if(j<Timing.Count&&locationsList[i].Order== Timing[j].LocationsOrder)
						{
						i++;
						j++;
						}
					else
						{
						var Insert=new ServiceInstanceTimingModel();
						Insert.LocationId= locationsList[i].Id;
						Insert.LocationName= locationsList[i].LocationName;
						Insert.LocationAbbrev= locationsList[i].LocationAbbreviation;
						Insert.EventType="";
						Insert.ArrivalTime= 0;
						Insert.WaitTime=0;
						Insert.TimeString="--";
						Insert.LocationsOrder= locationsList[i].Order;
						Insert.TimeEventId= -1; // TimeEvent is not valid!
						Timing.Insert(j,Insert);
						i++;
						j++;
						}
					}
				matrixModel.TimingList.Add(Timing);
				}

			for(int i=0;i<columncount-1;i++) // for each serviceInstance
				{
				int actualTime = serviceInstanceList[i].StartTime;
				var timing= matrixModel.TimingList[i];
				for (int j=0;j<locationsCount;j++) // for each location
					{
					if(timing[j].TimeEventId>0)
						{
						actualTime += timing[j].ArrivalTime;
						timing[j].TimeString= TimeConverters.TimeEventToString(actualTime,timing[j].WaitTime);
						actualTime += timing[j].WaitTime;
						}
					matrixModel.Matrix[j+1][i+1]= timing[j].TimeString;
					}
				}
			return matrixModel;
			}



		// Query to get toe serviceInstanceDetails


			public static List<ServiceInstanceTimingModel> GetServiceInstanceTiming(int serviceInstanceId, List<LocationModel> locations)
			{
			var sql = @"
						select Locations.Id as LocationId, 
						TimeEvents.id as TimeEventId, 
						Locations.LocationName as LocationName,
						Locations.LocationAbbreviation as LocationAbbrev,
						Locations.[Order] AS LocationsOrder,
						TimeEvents.EventType as EventType,
						TimeEvents.ArrivalTime as ArrivalTime,
						TimeEvents.WaitTime as WaitTime,
						'--' as TimeString
								from Locations, timeevents
								where TimeEvents.LocationId=Locations.Id and
										timeevents.ServiceId=(
										select Services.Id 
												from Services, ServiceInstances 
												where Services.Id= ServiceInstances.ServiceId 
														and serviceinstances.id=@serviceInstanceId)
						order by Locations.[order] asc
						";

			var serviceInstanceTiming =
				SQLiteData.LoadData<ServiceInstanceTimingModel, dynamic>(sql, new { serviceInstanceId }, SQLiteData.GetConnectionString()).ToList();

			// Now we neet to complete the list, using 

			return serviceInstanceTiming;
			}

		// Convert matrix to data
		public static DataView MatrixToDataTable(string[][] matrix)
			{
			var myDataTable = new DataTable();
			var vals = matrix;
			foreach (var value in vals[0])
				{
				myDataTable.Columns.Add(value);
				}

			int length = vals.GetLength(0);
			for (int i = 1; i < length; i++)
				{
				myDataTable.Rows.Add(vals[i]);
				}
			return myDataTable.DefaultView;
			}


		public static string GetCsvData(string[][] matrix)
			{
			var output="";
			int rowCount= matrix.GetLength(0);
			int columnCount= matrix[0].GetLength(0);

			for(int i=0;i<rowCount;i++)
				{
				for(int j=0;j<columnCount;j++)
					{
					if(j<columnCount-1)
						{
						output += $"{matrix[i][j]},";
						}
					else
						{
						output += $"{matrix[i][j]}\n";
						}
					}
				}
			return output;
			}
		}
	}
