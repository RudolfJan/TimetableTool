using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Screen = Caliburn.Micro.Screen;

namespace TimeTableTool.Desktop.ViewModels
  {
  public class AboutViewModel : Screen
    {
    public string ApplicationName { get; set; } = "Timetable Tool";
    public string Description { get; set; } =
      "A tool that helps to develop, maintain and display time tables for train simulation programs.";
    public string Author { get; set; } = "Rudolf Heijink";
    public string Version { get; set; } = "0.1 alpha";
    public string VersionDate { get; set; } = "May 2020";
    public string Copyright { get; set; } = "(C) 2020 Rudolf Heijink";
    public string WebSite { get; set; } = "https://www.hollandhiking.nl/trainsimulator";

 
    public async Task CloseAboutScreen()
      {
      await TryCloseAsync();
      }
    }
  }
