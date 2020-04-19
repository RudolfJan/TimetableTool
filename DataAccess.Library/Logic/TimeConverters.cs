using DataAccess.Library.Models;
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

    // TODO check if this works, maybe markup must be more configurable
    // his intial setup uses a two line approach.
    public static string TimeEventToString(int startTime, TimeEventModel timeEvent)
      {
      var output= $"{MinutesToString(startTime+timeEvent.ArrivalTime)}";
      if(timeEvent.WaitTime>0)
        {
        output += $" A\n{MinutesToString(startTime+timeEvent.ArrivalTime+timeEvent.WaitTime)} D";
        }

      return output;
      }


        public static string TimeEventToString(int startTime, int waitTime, bool csvTarget)
      {
      var output= $"{MinutesToString(startTime)}";
      if(waitTime>0)
        {
        if(csvTarget)
          {
          output += $" A {MinutesToString(startTime+waitTime)} D";
          }
        else
          {
          output += $" A\n{MinutesToString(startTime+waitTime)} D";
          }
         }
      return output;
      }
    }
  }
