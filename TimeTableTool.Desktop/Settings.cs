using Microsoft.Extensions.Configuration;
using System.IO;

namespace TimeTableTool.Desktop
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

    public static string ManualFolder
      {
      get { return $"{_config["DataConfig:DataPath"] + _config["DataConfig:ManualPath"]}"; }
      }

    public static string DatabasePath
      {
      get
        {
        return $"{_config["DataConfig:DataPath"] + _config["DataConfig:Database"]}";
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
