using Caliburn.Micro;
using DataAccess.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTableTool.Desktop.Models
  {
  public class TimeEventUIModel
    {
    public BindableCollection<LocationModel> LocationList { get; set; }= new BindableCollection<LocationModel>();
    public BindableCollection<FullTimeEventModel> FullTimeEventList { get; set; } = new BindableCollection<FullTimeEventModel>();
    public BindableCollection<FullTimeEventModel> FilteredFullTimeEventList { get; set; } = new BindableCollection<FullTimeEventModel>();
    public FullTimeEventFilterModel Filter { get; set; } = new FullTimeEventFilterModel();
    public ServiceModel SelectedService { get; set; }
    }
  }
