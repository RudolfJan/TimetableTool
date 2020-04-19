using Logging.Library;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.ServiceModel.Syndication;
using System.Windows.Documents;
using TimetableTool.Desktop.Helpers;

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

    public static string DataPath
      {
      get
        {
        return _config["DataConfig:DataPath"];
        }
      }

    public static string ManualPath
      {
      get { return $"{_config["DataConfig:DataPath"]}{_config["DataConfig:Manual"]}"; }
      }

    public static string DatabasePath
      {
      get
        {
        return $"{_config["DataConfig:DataPath"] + _config["DataConfig:Database"]}";
        }
      }

    public static bool UseDemoData
    {
    get
      {
       bool _useDemoData=false;
       var result= bool.TryParse($"{_config["DataConfig:UseDemoData"]}",out _useDemoData);
        if(!result)
          {
          Log.Trace("Configuration error in appsettings.json, UseDemoData is not a valid boolean. Proceed with value true", LogEventType.Error);
          _useDemoData=true;
          }
      return _useDemoData;
      }
    }


    public static string ConnectionString
      {
      get
        {
        return _config["ConnectionStrings:SqLiteDb"].Replace("path",DatabasePath);
        }
      }

    }
  }
