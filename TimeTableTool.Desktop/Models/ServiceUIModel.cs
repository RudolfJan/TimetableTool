using DataAccess.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TimeTableTool.Desktop.Models
  {
  public class ServiceUIModel
    {
    public BindingList<ServiceModel> ServiceList { get; set; }
    public ServiceModel ServiceDetails { get; set; }= new ServiceModel();
    public ServiceFilterModel Filter { get; set; } = new ServiceFilterModel();
    public string RouteName { get; set; }
    }
  }
