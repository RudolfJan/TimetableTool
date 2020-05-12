using Caliburn.Micro;
using DataAccess.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TimetableTool.Desktop.Models
  {
  public class ServiceTemplateUIModel
    {
    public BindableCollection<ServiceTemplateModel> ServiceTemplateList { get; set; }
    public ServiceTemplateFilterModel Filter { get; set; } = new ServiceTemplateFilterModel();
    public string RouteName { get; set; }
    }
  }
