using DataAccess.Library.Models;
using SQLiteDataAccess.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Library.Logic
	{
	public class TrainDataAccess
		{
		public static List<TrainModel> GetAllTrainsPerRoute(int RouteId)
			{
			string sql = "SELECT * FROM Trains WHERE RouteId=@RouteId";

			var trainList =
				SQLiteData.LoadData<TrainModel, dynamic>(sql, new { RouteId }, SQLiteData.GetConnectionString()).ToList();
			return trainList;
			}

		public static TrainModel GetTrainById(int trainId)
			{
			string sql = "SELECT * FROM Trains WHERE Id= @TrainId";

			var train =
				SQLiteData.LoadData<TrainModel, dynamic>(sql, new { trainId }, SQLiteData.GetConnectionString()).FirstOrDefault();
			return train;
			}

		public static int InsertTrain(TrainModel train)
			{
			string sql = "INSERT OR IGNORE INTO Trains (TrainName, TrainAbbreviation, TrainDescription, TrainClass, RouteId) VALUES(@TrainName, @TrainAbbreviation, @TrainDescription, @TrainClass, @RouteId);SELECT last_insert_rowid();";
			return SQLiteData.SaveData<dynamic>(sql, new { train.TrainName, train.TrainAbbreviation, train.TrainDescription, train.TrainClass, train.RouteId }, SQLiteData.GetConnectionString());
			}

		public static void UpdateTrain(TrainModel train)
			{
			string sql = "UPDATE OR IGNORE Trains SET TrainName=@TrainName, TrainAbbreviation=@TrainAbbreviation, TrainDescription=@TrainDescription WHERE Id= @Id";
			SQLiteData.SaveData<dynamic>(sql, new { train.TrainName, train.TrainAbbreviation, train.TrainDescription, train.Id }, SQLiteData.GetConnectionString());
			}

		public static void DeleteTrain(int trainId)
			{
			string sql = "PRAGMA foreign_keys = ON;DELETE FROM Trains WHERE Trains.Id=@TrainId;";
			SQLiteData.SaveData<dynamic>(sql, new { trainId }, SQLiteData.GetConnectionString());
			}
		}
	}
