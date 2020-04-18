using Logging.Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace TimetableTool.Desktop.Models
	{
	public class LoggingModel
		{
	  public ILogCollectionManager Logging { get; set; }
    public ILogCollectionManager FilteredLogging { get; set; }
    public LogFilter Filter { get; set; } = new LogFilter(false,true,true,false);
		}
	}
