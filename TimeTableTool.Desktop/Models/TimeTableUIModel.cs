using DataAccess.Library.Models;
using System.ComponentModel;

namespace TimetableTool.Desktop.Models
  {
  public class TimetableUIModel
    {
    public BindingList<TimetableModel> TimetableList { get; set; }
    public TimetableFilterModel Filter { get; set; } = new TimetableFilterModel();
    public string RouteName { get; set; }
    }
  }
