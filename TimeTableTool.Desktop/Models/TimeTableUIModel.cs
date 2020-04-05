using DataAccess.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TimeTableTool.Desktop.Models
  {
  public class TimeTableUIModel
    {
    public BindingList<TimeTableModel> TimeTableList { get; set; }
    public TimeTableModel TimeTableDetails { get; set; }= new TimeTableModel();
    public TimeTableFilterModel Filter { get; set; } = new TimeTableFilterModel();
    public string RouteName { get; set; }
    }
  }
