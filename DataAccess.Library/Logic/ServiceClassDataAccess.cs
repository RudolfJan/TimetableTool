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
			var sql="SELECT Id, ServiceClassname, ServiceClassDescription, Category FROM \"ServiceClasses\";";
			var serviceClassList =
				SQLiteData.LoadData<ServiceClassModel, dynamic>(sql, new {}, SQLiteData.GetConnectionString()).ToList();
			return serviceClassList;
			}

		public static ServiceClassModel GetServiceClassModelFromString(string serviceClass, List<ServiceClassModel> serviceClassList)
			{
			ServiceClassModel output = serviceClassList.First(x => x.ServiceClassName == serviceClass);
			if (output == null)
				{
				ServiceClassModel defaultValue= new ServiceClassModel();
				defaultValue.ServiceClassName = "Invalid service class";
				defaultValue.ServiceClassDescription = "Choose from the predefined list";
				output = defaultValue;
				}
			return output;
			}
		}
	}
