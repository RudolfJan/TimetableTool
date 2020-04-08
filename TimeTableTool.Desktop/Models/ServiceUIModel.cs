using DataAccess.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TimetableTool.Desktop.Models
  {
  public class ServiceUIModel
    {
    public BindingList<ServiceModel> ServiceList { get; set; }
    public ServiceFilterModel Filter { get; set; } = new ServiceFilterModel();
    public string RouteName { get; set; }
    }
  }
