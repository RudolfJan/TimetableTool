using Logging.Library;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace TimetableTool.Desktop
	{
	public static class Settings
		{
		private static readonly IConfiguration _config = CreateConfig();

		static IConfiguration CreateConfig()
			{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json");
			IConfiguration config = builder.Build();
			
			return config;
			}

		public static int DatabaseVersion { get; set; }
		public static string MyDocumentsFolder
			{
			get
				{
				var result = bool.TryParse($"{_config["DataConfig:UseMyDocumentsFolder"]}",
					out var _useMyDocumentsFolder);
				if (!result)
					{
					Log.Trace(
						"Configuration error in appsettings.json, UseMyDocumentsFolder is not a valid boolean. Proceed with value false",
						LogEventType.Error);
					}

				if (_useMyDocumentsFolder)
					{
					try
						{
						var folder=Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
						return $"{folder}\\";
						}
					catch (Exception ex)
						{
						Log.Trace("Cannot set special folder", ex, LogEventType.Error);
						throw;
						}
					}
				return string.Empty;
				}
			}


		public static string DataPath
			{
			get
				{
				var output= $"{MyDocumentsFolder}{ _config["DataConfig:DataPath"]}";
				return output;
				}
			}

		public static string ManualPath
			{
			get { return $"{DataPath}{_config["DataConfig:Manual"]}"; }
			}

		public static string BackupPath
			{
			get { return $"{DataPath}{_config["DataConfig:BackupPath"]}"; }
			}

		public static string DatabasePath
			{
			get
				{
				return $"{DataPath}{_config["DataConfig:Database"]}";
				}
			}

		public static bool UseDemoData
			{
			get
				{
				bool _useDemoData = false;
				var result = bool.TryParse($"{_config["DataConfig:UseDemoData"]}", out _useDemoData);
				if (!result)
					{
					Log.Trace("Configuration error in appsettings.json, UseDemoData is not a valid boolean. Proceed with value true", LogEventType.Error);
					_useDemoData = true;
					}
				return _useDemoData;
				}
			}


		public static string ConnectionString
			{
			get
				{
				return _config["ConnectionStrings:SqLiteDb"].Replace("path", DatabasePath);
				}
			}

		}
	}
