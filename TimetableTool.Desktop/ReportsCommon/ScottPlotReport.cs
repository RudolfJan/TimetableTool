using DataAccess.Library.Models;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace TimetableTool.Desktop.ReportsCommon
	{
	public class ScottPlotReport


		{
		#region Configuration

		public static void ConfigurePlot(WpfPlot plot)
			{
			plot.plt.Style(figBg: ScottPlotReport.GetResourceColor("WindowBackground"));
			plot.plt.Style(dataBg: ScottPlotReport.GetResourceColor("ControlBackground"));
			plot.plt.Grid(enable: true, color: ScottPlotReport.GetResourceColor("GridLine"));
			plot.Configure(enablePanning: false, enableScrollWheelZoom: false, enableRightClickZoom: false);
			}

		#endregion

		#region LocationAxis
		public static void PlotLocationAxis(Plot plot, ObservableCollection<LocationModel> LocationList, double xAxisLabelRotation)
			{
			var labels = DefineLocationLabels(LocationList);
			plot.Ticks(xTickRotation: xAxisLabelRotation);
			plot.XTicks(labels);
			}

		private static string[] DefineLocationLabels(ObservableCollection<LocationModel> LocationList)
			{
			string[] labels = new string[LocationList.Count];
			int k = 0;
			foreach (var label in LocationList)
				{
				labels[k] = label.LocationAbbreviation;
				k++;
				}
			return labels;
			}

		#endregion

		#region TimeAxis
		public static void PlotTimeAxis(Plot plot, double zoom, double pan)
			{
			double[] yPositions = CreateTimeAxisPositions();
			string[] yLabels = CreateTimeAxisLabels();
			plot.Ticks(invertSignY: true);
			plot.YTicks(yPositions, yLabels);
			plot.AxisPan(dy: -pan * 100);
			plot.AxisZoom(yFrac: zoom);
			}

		public static string[] CreateTimeAxisLabels()
			{
			var yLabels = new string[25];
			for (int p = 0; p < 25; p++)
				{
				yLabels[p] = p.ToString("D2") + ":00";
				}
			return yLabels;
			}

		public static double[] CreateTimeAxisPositions()
			{
			var yPositions = new double[25];
			for (int p = 0; p < 25; p++)
				{
				yPositions[p] = -60 * p;
				}
			return yPositions;
			}

		#endregion

		#region ColorConverters

		public static Color GetResourceColor(string resourceName)
			{
			var BackgroundColorBrush =
				Application.Current.TryFindResource(resourceName) as SolidColorBrush;
			if (BackgroundColorBrush != null)
				{
				return ToDrawingColor(BackgroundColorBrush.Color);
				}
			return Color.Aqua; // Ugly default value
			}

		public static Color ToDrawingColor(System.Windows.Media.Color mediaColor)
			{
			return Color.FromArgb(mediaColor.A, mediaColor.R, mediaColor.G, mediaColor.B);
			}

		#endregion

		#region Marker
		public static PlottableScatter DrawMarker(Plot plot, PlottableScatter marker, double x, double y)
			{
			double[] xs = {x};
			double[] ys = {y};
			if (marker != null)
				{
				marker.markerShape = MarkerShape.none;
				}
			marker = plot.PlotScatter(xs, ys, Color.Red,
				markerShape: MarkerShape.openCircle, markerSize: 15);
			return marker;
			}
		#endregion
		}
	}
