using Caliburn.Micro;
using Logging.Library;
using System;
using System.Threading.Tasks;
using TimetableTool.Desktop.Models;

namespace TimetableTool.Desktop.ViewModels
  {
  public class LoggingViewModel: Screen
		{
    ILogCollectionManager _logger;
    public LoggingModel Logging { get; set; }= new LoggingModel();
    public bool _debugLogging =true;
    public bool DebugLogging
      {
      get { return _debugLogging; }
      set
        {
        _debugLogging = value;
        NotifyOfPropertyChange(()=>DebugLogging);
        Logging.Filter.DebugChecked = DebugLogging;
        ChangeFilter();
        }
      }

    public bool _messageLogging =true;
    public bool MessageLogging
      {
      get { return _messageLogging; }
      set
        {
        _messageLogging = value;
        NotifyOfPropertyChange(() => MessageLogging);
        Logging.Filter.MessageChecked = MessageLogging;
        ChangeFilter();
        }
      }

    public bool _errorLogging =true;
    public bool ErrorLogging
      {
      get { return _errorLogging; }
      set
        {
        _errorLogging = value;
        NotifyOfPropertyChange(() => ErrorLogging);
        Logging.Filter.ErrorChecked = ErrorLogging;
        ChangeFilter();
        }
      }

    public bool _eventLogging = true;
    public bool EventLogging
      {
      get { return _eventLogging; }
      set
        {
        _eventLogging = value;
        NotifyOfPropertyChange(() => EventLogging);
        Logging.Filter.EventChecked = EventLogging;
        ChangeFilter();
        }
      }

    public LoggingViewModel(ILogCollectionManager logger)
      {
      _logger=logger;

      }

      protected override void OnViewLoaded(object view)
      {
      base.OnViewLoaded(view);
      Logging.Logging= _logger;
      Logging.FilteredLogging= new BindableCollection<LogEntryClass>();
      CreateTestData();
      // Setup initial values for logging filter
      ChangeFilter();
      LogEventHandler.LogEvent += LogEventHandlerOnLogEvent;
      }

    private void LogEventHandlerOnLogEvent(Object sender, LogEventArgs e)
      {
      ChangeFilter();
      }

    private void CreateTestData()
      {
      Logging.Logging.LogEvents.Add(new LogEntryClass("","",1,"Test line Message",null,LogEventType.Message));
      Logging.Logging.LogEvents.Add(new LogEntryClass("","",2,"Test line Message",null,LogEventType.Message));
      Logging.Logging.LogEvents.Add(new LogEntryClass("","",2,"Test line Debug",null,LogEventType.Debug));
      Logging.Logging.LogEvents.Add(new LogEntryClass("","",2,"Test line Error",null,LogEventType.Error));
      Logging.Logging.LogEvents.Add(new LogEntryClass("","",2,"Test line Error",null,LogEventType.Error));
      Logging.Logging.LogEvents.Add(new LogEntryClass("","",2,"Test line Event",null,LogEventType.Event));
      Logging.Logging.LogEvents.Add(new LogEntryClass("","",2,"Test line Error",null,LogEventType.Error));
      }

    private void ChangeFilter()
      {
      Logging.FilteredLogging.Clear();
      foreach (var item in Logging.Logging.LogEvents)
        {
        if (Logging.Filter.EventTypeFilter(item))
          {
          Logging.FilteredLogging.Add(item);
          }
        }
      }

    private void ClearLog()
      {
      Logging.FilteredLogging.Clear();
      Logging.Logging.LogEvents.Clear();
      NotifyOfPropertyChange(() => Logging.FilteredLogging);
      }

     public void SaveLog()
      {
      // TODO

           //var FileDialog = new SaveFileDialog
      //  {
        //InitialDirectory = CLuaCreatorOptions.LuaCreatorDirectory + "Temp",
        //  RestoreDirectory = true,
        //  Title = "Save log file",
        //  Filter = "Text file (*.txt)|*.txt|All files (*.*)|*.*"
        //  };

        //if (FileDialog.ShowDialog() == true)
        //  {
        //  var AllText = String.Empty;
        //  foreach (var X in Log.LogManager)
        //    {
        //    AllText += X + "\r\n";
        //    }

        //  File.WriteAllText(FileDialog.FileName, AllText);
        //  }



      }
    public async Task OKButton()
      {
      await TryCloseAsync();
      }

		}
	}
