using Caliburn.Micro;
using DataAccess.Library.Models;

namespace TimetableTool.Desktop.Models
  {
  public class TimeEventUIModel
    {
    public BindableCollection<LocationModel> LocationList { get; set; }= new BindableCollection<LocationModel>();
    public BindableCollection<FullTimeEventModel> FullTimeEventList { get; set; } = new BindableCollection<FullTimeEventModel>();
    public BindableCollection<FullTimeEventModel> FilteredFullTimeEventList { get; set; } = new BindableCollection<FullTimeEventModel>();
    public FullTimeEventFilterModel Filter { get; set; } = new FullTimeEventFilterModel();
    public ServiceTemplateModel SelectedService { get; set; }
    public RouteModel SelectedRoute { get; set; }
    }
  }
