using Logging.Library;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Forms;

namespace TimetableTool.Desktop
	{
	// https://stackoverflow.com/questions/51351464/user-configuration-settings-in-net-core

	// See http://geekswithblogs.net/BlackRabbitCoder/archive/2010/05/19/c-system.lazylttgt-and-the-singleton-design-pattern.aspx

	public class Settings
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

		// static holder for instance, need to use lambda to construct since constructor private
		private static readonly Lazy<Settings> MyInstance
			= new Lazy<Settings>(() => new Settings());

		// private to prevent direct instantiation.

		private Settings()
			{
			}

		// accessor for instance

		public static Settings Instance
			{
			get
				{
				return MyInstance.Value;
				}
			}

		public static string RegkeyString
			{
			get { return "software\\Holland Hiking\\TimetableTools"; }
			}

		private static RegistryKey OpenRegistry()
			{
			return Registry.CurrentUser.CreateSubKey(RegkeyString, true);
			}

		#region DataAccess

		// User settings, a user may change go in the user settings.
		// Other configuration stuff goes in the appsettings.json file
		public static void ReadFromRegistry()
			{
			using var AppKey = OpenRegistry();
			ScottPlotWidth = int.Parse((string) AppKey.GetValue(nameof(ScottPlotWidth), _config["Interface:ScottPlotWidth"]));
			ScottPlotHeight = int.Parse((string) AppKey.GetValue(nameof(ScottPlotHeight), _config["Interface:ScottPlotHeight"]));
			}

		public static void WriteToRegistry()
			{
			using var AppKey = OpenRegistry();
			AppKey.SetValue(nameof(ScottPlotWidth), ScottPlotWidth, RegistryValueKind.String);
			AppKey.SetValue(nameof(ScottPlotHeight), ScottPlotHeight, RegistryValueKind.String);
			}

#endregion
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


		#region Values

		public static int ScottPlotWidth { get; set; }
		public static int ScottPlotHeight { get; set; }
		
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
		#endregion
		}
	}
