using Caliburn.Micro;
using Logging.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimetableTool.Desktop.Models;

namespace TimetableTool.Desktop.ViewModels
	{
	public class LoggingViewModel: Screen
		{
    ILogCollectionManager _logger;
    public LoggingModel Logging { get; set; }= new LoggingModel();

    public LoggingViewModel(ILogCollectionManager logger)
      {
      _logger=logger;
      Logging.Logging= _logger;
      Logging.FilteredLogging= _logger;
      
      }

      protected override void OnViewLoaded(object view)
      {
      base.OnViewLoaded(view);
      CreateTestData();
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
      Logging.FilteredLogging.LogEvents = GetFilteredLogging(Logging.Filter, Logging.Logging);
      }

    private void ClearLog()
      {
      Logging.Logging.LogEvents.Clear();
      Logging.FilteredLogging.LogEvents.Clear();
      }

    static List<LogEntryClass> GetFilteredLogging(LogFilter filter,ILogCollectionManager logging)
      {
      return logging.LogEvents.Where(filter.EventTypeFilter).ToList();
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
