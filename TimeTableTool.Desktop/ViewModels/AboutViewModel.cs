using System.Threading.Tasks;
using Caliburn.Micro;

namespace TimetableTool.Desktop.ViewModels
  {
  public class AboutViewModel : Screen
    {
    public string ApplicationName { get; set; } = "Timetable Tool";
    public string Description { get; set; } =
      "A tool that helps to develop, maintain and display timetables for (train) simulation programs.";
    public string Author { get; set; } = "Rudolf Heijink";
    public string Version { get; set; } = "0.2.0 alpha";
    public string VersionDate { get; set; } = "May 2020";
    public string Copyright { get; set; } = "(C) 2020 Rudolf Heijink";
    public string WebSite { get; set; } = "https://www.hollandhiking.nl/trainsimulator";

 
    public async Task CloseAboutScreen()
      {
      await TryCloseAsync();
      }
    }
  }
