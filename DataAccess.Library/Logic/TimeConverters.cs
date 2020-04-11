using System;
using System.Collections.Generic;
using System.Text;

namespace TimetableTool.DataAccessLibrary.Logic
  {
  public class TimeConverters
    {
    // input is time in minutes!
    public static string MinutesToString(int input)
      {
      string output;
      int hours;
      int minutes;

      hours = input / 60;
      minutes = input - hours * 60;
      output = $"{hours:00}:{minutes:00}";
      return output;
      }

    // input in format hh:mm, no validation on input!
    public static int TimeToMinutes(string input)
      {
      string hoursText = input.Substring(0, 2);
      string minutesText = input.Substring(3);
      int output = Convert.ToInt32(hoursText) * 60 + Convert.ToInt32(minutesText);
      return output;
      }

    }
  }
