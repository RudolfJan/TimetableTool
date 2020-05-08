using Caliburn.Micro;
using DataAccess.Library.Logic;
using Logging.Library;
using SQLiteDataAccess.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TimetableTool.Desktop.ViewModels;
using TimetableTool.Desktop.Views;
using System.Threading.Tasks;

namespace TimetableTool.Desktop
  {
  public class Bootstrapper : BootstrapperBase
    {
    private readonly SimpleContainer _container = new SimpleContainer();

    public Bootstrapper()
      {
      Initialize();
      }

    protected override void Configure()
      {
      _container
        .Singleton<IWindowManager, WindowManager>()
        .Singleton<IEventAggregator, EventAggregator>();

      GetType().Assembly.GetTypes()
          .Where(type => type.IsClass)
          .Where(type => type.Name.EndsWith("ViewModel"))
          .ToList()
          .ForEach(viewModelType => _container.RegisterPerRequest(
              viewModelType, viewModelType.ToString(), viewModelType));
      }

    protected override void OnStartup(object sender, StartupEventArgs e)
      {
      LogEventHandler.LogEvent += OnLogEvent;
     int databaseExists=SQLiteData.InitDatabase(Settings.ConnectionString,Settings.DatabasePath,Settings.UseDemoData);
      Settings.DatabaseVersion = VersionDataAccess.GetCurrentDatabaseVersion();
      if (Settings.DatabaseVersion < 2)
        {
        Log.Trace("Deprecated database version. Check user manual ch3.2 for a solution.",
          LogEventType.Event);
        }
      if (Settings.UseDemoData && databaseExists==0)
        {
        var importHH= new ImportRouteDataAccess("SQL\\HH-testset.ttt");
        var importWSR= new ImportRouteDataAccess("SQL\\WSR-testset.ttt");
        }
      DisplayRootViewFor<ShellViewModel>();
      }

    private void OnLogEvent(Object Sender, LogEventArgs args)
      {
      if (args.EntryClass.EventType == LogEventType.Error || args.EntryClass.EventType == LogEventType.Event)
        {
        LogCollectionManager.LogEvents.Add(args.EntryClass);
        var message = args.EntryClass.LogEntry;
        var form = new NotificationView(message);
        form.Show();
        }
      }

    protected override object GetInstance(Type service, string key)
      {
      return _container.GetInstance(service, key);
      }

    protected override IEnumerable<object> GetAllInstances(Type service)
      {
      return _container.GetAllInstances(service);
      }

    protected override void BuildUp(object instance)
      {
      _container.BuildUp(instance);
      }
    }
  }
