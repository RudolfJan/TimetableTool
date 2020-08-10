using DataAccess.Library.Models;
using SQLiteDataAccess.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Library.Logic
	{
	public class TrainServiceDataAccess
		{
		public static List<TrainServiceModel> GetTrainServicesPerTrain(int trainId)
			{
			string sql = @"SELECT
                        TrainServices.Id AS Id,
												TrainServices.ServiceId AS ServiceId,
												TrainServices.TrainId as TrainId
                      FROM TrainServices WHERE TrainServices.TrainId=@trainid";

			var serviceList =
				SQLiteData.LoadData<TrainServiceModel, dynamic>(sql, new { trainId }, SQLiteData.GetConnectionString()).ToList();
			return serviceList;
			}

		public static int InsertTrainService(TrainServiceModel trainService)
			{
			string sql = @"INSERT OR IGNORE INTO TrainServices (ServiceId, TrainId) 
                                    VALUES(@ServiceId, @TrainId);SELECT last_insert_rowid();";
			return SQLiteData.SaveData<dynamic>(sql,
				new
					{
					trainService.ServiceId,
					trainService.TrainId
					},
				SQLiteData.GetConnectionString());
			}

		public static void DeleteTrainService(TrainServiceModel trainService)
			{
			string sql = "PRAGMA foreign_keys = ON;DELETE FROM TrainServices WHERE TrainServices.TrainId=@TrainId AND TrainServices.ServiceId= @ServiceId;";
			SQLiteData.SaveData<dynamic>(sql, new { trainService.TrainId, trainService.ServiceId }, SQLiteData.GetConnectionString());
			}

		public static void UpdateTrainService(TrainServiceModel trainService)
			{
			string sql = "UPDATE OR IGNORE TrainServices SET ServiceId=@ServiceId, trainId=@TrainId WHERE Id=@Id";
			SQLiteData.SaveData<dynamic>(sql,
				new
					{
					trainService.ServiceId,
					trainService.TrainId,
					trainService.Id
					},
				SQLiteData.GetConnectionString());
			}

		public static List<TrainServiceModel> GetAllTrainServicesPerRoute(int routeId)
			{
			string sql = @"SELECT
                        TrainServices.Id AS Id,
												TrainServices.ServiceId AS ServiceId,
												TrainServices.TrainId as TrainId
                      FROM TrainServices, Trains WHERE TrainServices.TrainId=Trains.Id AND Trains.RouteId= @routeid;";

			var serviceList =
				SQLiteData.LoadData<TrainServiceModel, dynamic>(sql, new { routeId }, SQLiteData.GetConnectionString()).ToList();
			return serviceList;
			}
		}
	}
