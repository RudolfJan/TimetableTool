using Caliburn.Micro;
using Logging.Library;
using SQLiteDataAccess.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TimetableTool.Desktop.ViewModels;
using TimetableTool.Desktop.Views;

namespace TimetableTool.Desktop
  {
  public class Bootstrapper : BootstrapperBase
    {
    private SimpleContainer _container = new SimpleContainer();

    public Bootstrapper()
      {
      Initialize();
      }

    protected override void Configure()
      {
      LogCollectionManager logger= new LogCollectionManager();
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

      SQLiteData.InitDatabase(Settings.ConnectionString,Settings.DatabasePath);
      LogEventHandler.LogEvent += OnLogEvent;
      DisplayRootViewFor<ShellViewModel>();
      }

    private void OnLogEvent(Object Sender, LogEventArgs args)
      {
      if (args.EntryClass.EventType == LogEventType.Error)
        {
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
