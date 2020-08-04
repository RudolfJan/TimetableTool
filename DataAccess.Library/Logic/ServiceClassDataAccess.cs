using DataAccess.Library.Models;
using SQLiteDataAccess.Library;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Library.Logic
	{
	public class ServiceClassDataAccess
		{
		public static List<ServiceClassModel> GetAllServiceClasses()
			{
			var sql = "SELECT Id, ServiceClassname, ServiceClassDescription, Category, Color FROM \"ServiceClasses\";";
			var serviceClassList =
				SQLiteData.LoadData<ServiceClassModel, dynamic>(sql, new { }, SQLiteData.GetConnectionString()).ToList();
			return serviceClassList;
			}

		public static ServiceClassModel GetServiceClassModelFromString(string serviceClass, List<ServiceClassModel> serviceClassList)
			{
			var cl1 = serviceClass.ToLower();
			foreach (var item in serviceClassList)
				{
				var cl2 = item.ServiceClassName.ToLower();
				if(cl1==cl2)
					{
					return item;
					}
				}

			ServiceClassModel defaultValue = new ServiceClassModel();
			defaultValue.ServiceClassName = "Invalid service class";
			defaultValue.ServiceClassDescription = "Choose from the predefined list";
			defaultValue.Color = "Magenta";
			return defaultValue;
			}
		}
	}
