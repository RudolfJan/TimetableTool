using DataAccess.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace TimetableTool.Desktop.EventModels
	{
	public enum ReportType
		{
		Classic, Graph, ScottPlotGraph, ArrivalDeparture, TrainPlanning, ConsistencyChecks
		}
	public class ReportSelectedEvent
		{
		public TimetableRouteModel SelectedTimetable { get; set; }
		public ReportType Report { get; set; }
		}
	}
