using DataAccess.Library.Models;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TimeTableTool.Desktop.Models
  {
  public class RouteUIModel
    {
    public BindingList<RouteModel> RouteList { get; set; }
    public RouteModel RouteDetails { get; set; }= new RouteModel();
    public RouteFilterModel Filter { get; set; } = new RouteFilterModel();
    }
  }
