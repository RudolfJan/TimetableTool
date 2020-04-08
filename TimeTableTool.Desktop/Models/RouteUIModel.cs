using DataAccess.Library.Models;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TimetableTool.Desktop.Models
  {
  public class RouteUIModel
    {
    public BindingList<RouteModel> RouteList { get; set; }
    public RouteFilterModel Filter { get; set; } = new RouteFilterModel();
    }
  }
