using DataAccess.Library.Logic;
using DataAccess.Library.Models;
using ScottPlot;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TimetableTool.DataAccessLibrary.Logic;
using TimetableTool.Desktop.Models;
using TimetableTool.Desktop.ViewModels;
using Application = System.Windows.Application;
using Color = System.Drawing.Color;
using TextAlignment = ScottPlot.TextAlignment;

namespace TimetableTool.Desktop.Views
	{
	/// <summary>
	/// Interaction logic for ScottPlotGraphView.xaml
	/// </summary>
	public partial class ScottPlotGraphForm
		{
		
		public ScottPlotGraph GraphModel { get; set; }
		private ScottPlot.PlottableScatterHighlight scatterHighLight;
		private double[] scatterDataX;
		private double[] scatterDataY;
		private TimeGraphUIModel[] highlightedTimeGraphUiModels;
		private int scatterIndex = 0;
		private PlottableScatter marker;

		public ScottPlotGraphForm(ScottPlotGraph graphModel)
			{
			InitializeComponent();
			GraphModel = graphModel;
			DataContext = GraphModel;
			PlotGraph(GraphModel);
			}

		public void PlotGraph(ScottPlotGraph graphModel)
			{
			var locationCount = GraphModel.LocationList.Count;
			var serviceCount = GraphModel.TimeGraphUI.Count;
			var pointCount = locationCount * serviceCount;
			scatterDataX = new double[pointCount + 1];
			scatterDataY = new double[pointCount + 1];
			highlightedTimeGraphUiModels = new TimeGraphUIModel[pointCount + 1];
			scatterIndex = 0;
			int i = 0;
			foreach (var graph in GraphModel.TimeGraphUI)
				{
				graph.Plot = PlotService(graphModel, graph, GraphModel.TimeGraphUI[i].ServiceAbbreviation);
				i++;
				}

			PlotTimeAxis(graphModel.Zoom, graphModel.Pan);
			PlotLocationAxis(graphModel);

			if (marker != null)
				{
				//redraw marker
				marker = TimetableGraph.plt.PlotScatter(marker.xs, marker.ys, Color.Red,
					markerShape: MarkerShape.openCircle, markerSize: 15);
				}

			if (GraphModel.SelectedTimeGraph != null)
				{
				GraphModel.SelectedTimeGraph.Plot.lineWidth = 3;
				}
			
			TimetableGraph.plt.Style(figBg: GetResourceColor("WindowBackground"));
			TimetableGraph.plt.Style(dataBg: GetResourceColor("ControlBackground"));
			TimetableGraph.plt.Grid(enable: true, color: GetResourceColor("GridLine"));
			scatterHighLight = TimetableGraph.plt.PlotScatterHighlight(scatterDataX, scatterDataY, lineWidth: 0, markerSize: 0.5);
			TimetableGraph.Configure(enablePanning: false, enableScrollWheelZoom: false, enableRightClickZoom: false);
			TimetableGraph.Render();
			}


		public void PlotTimeAxis(double zoom, double pan)
			{
			double[] yPositions = new double[25];
			string[] yLabels = new string[25];
			for (int p = 0; p < 25; p++)
				{
				yPositions[p] = -60 * p;
				yLabels[p] = p.ToString("D2") + ":00";
				}
			TimetableGraph.plt.Ticks(invertSignY: true);
			TimetableGraph.plt.YTicks(yPositions, yLabels);
			TimetableGraph.plt.AxisPan(dy: -pan * 100);
			TimetableGraph.plt.AxisZoom(yFrac: zoom);
			}

		public void PlotLocationAxis(ScottPlotGraph graphModel)
			{
			string[] labels = new string[graphModel.LocationList.Count];
			int k = 0;
			foreach (var label in graphModel.LocationList)
				{
				labels[k] = label.LocationAbbreviation;
				k++;
				}
			TimetableGraph.plt.XTicks(labels);
			}


		public PlottableScatter PlotService(ScottPlotGraph model, TimeGraphUIModel graph, string serviceAbbreviation)
			{
			int startTime = TimeConverters.TimeToMinutes(model.StartTimeText);
			int endTime = TimeConverters.TimeToMinutes(model.EndTimeText);
			double[] dataX = new double[graph.DataLine.Count];
			double[] dataY = new double[graph.DataLine.Count];

			int j = 0;
			if (graph.DataLine[0].Y >= startTime && graph.DataLine[0].Y < endTime)
				{
				foreach (var dataSet in graph.DataLine)
					{
					dataX[j] = dataSet.X;
					dataY[j] = -dataSet.Y;
					scatterDataX[scatterIndex] = dataSet.X;
					scatterDataY[scatterIndex] = -dataSet.Y;
					highlightedTimeGraphUiModels[scatterIndex] = graph;
					scatterIndex++;
					j++;
					}


				double xOffset = 0;
				double yOffset = 5;
				TextAlignment alignment;
				if (dataX[0] < dataX.GetLength(0) / 2)
					{
					alignment = TextAlignment.upperLeft;
					}
				else
					{
					alignment = TextAlignment.upperRight;
					}
				TimetableGraph.plt.PlotText(serviceAbbreviation,
					dataX[0] + xOffset, dataY[0] + yOffset, fontName: "Arial", fontSize: 10,
					color: GetResourceColor("GraphText"), bold: true, alignment: alignment);
				return TimetableGraph.plt.PlotScatterHighlight(dataX, dataY, GetLineColor(graph.ServiceType));
				}
			else
				{
				return null;
				}
			}

		public System.Drawing.Color GetLineColor(string serviceType)
			{
			var serviceClass = ServiceClassDataAccess.GetServiceClassModelFromString(serviceType, GraphModel.ServiceClassList);
			if (serviceClass == null)
				{
				return Color.Magenta;
				}
			return System.Drawing.Color.FromName(serviceClass.Color);
			}

		public System.Drawing.Color GetResourceColor(string resourceName)
			{
			var BackgroundColorBrush =
				Application.Current.TryFindResource(resourceName) as SolidColorBrush;
			if (BackgroundColorBrush != null)
				{
				return ToDrawingColor(BackgroundColorBrush.Color);
				}
			return System.Drawing.Color.Aqua; // Ugly default value
			}

		public System.Drawing.Color ToDrawingColor(System.Windows.Media.Color mediaColor)
			{
			return System.Drawing.Color.FromArgb(mediaColor.A, mediaColor.R, mediaColor.G, mediaColor.B);
			}

		private void Exit_Click(object sender, RoutedEventArgs e)
			{
			Close();
			}

		private void Export_Click(object sender, RoutedEventArgs e)
			{
			// TODO create a better filename?
			TimetableGraph.plt.SaveFig($"{Settings.DataPath}Graph{GraphModel.Timetable.RouteId}-{GraphModel.Timetable.TimetableName}.png");
			}

		private void WindowLoaded(object sender, RoutedEventArgs e)
			{
			MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight * 0.9;
			MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
			}

		private void Redraw_OnClick(object sender, RoutedEventArgs e)
			{
			TimetableGraph.plt.Clear();
			PlotGraph(GraphModel);
			}

		private void OnZoomChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
			{
			if (GraphModel != null)
				{
				if (TimetableGraph?.plt != null)
					{
					TimetableGraph.plt.Clear();
					PlotGraph(GraphModel);
					}
				}
			}

		private void OnMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
			{
			var mousePos = e.MouseDevice.GetPosition(TimetableGraph);
			double mouseX = TimetableGraph.plt.CoordinateFromPixelX(mousePos.X);
			double mouseY = TimetableGraph.plt.CoordinateFromPixelY(mousePos.Y);
			scatterHighLight.HighlightClear();
			var (x, y, index) = scatterHighLight.HighlightPointNearest(mouseX, mouseY);
			TimeGraphUIModel graph = highlightedTimeGraphUiModels[index];
			if (graph != null)
				{
				double[] xs = { x };
				double[] ys = { y };
				if (marker != null)
					{
					marker.markerShape = MarkerShape.none;
					}

				marker = TimetableGraph.plt.PlotScatter(xs, ys, Color.Red,
						markerShape: MarkerShape.openCircle, markerSize: 15);
				if (GraphModel.SelectedTimeGraph != null && GraphModel.SelectedTimeGraph.Plot != null)
					{
					GraphModel.SelectedTimeGraph.Plot.lineWidth = 1;
					}
				graph.Plot.lineWidth = 3;
				GraphModel.SelectedTimeGraph = graph;
				TimetableGraph.Render();
				}
			TimetableGraph.Render();
			}
		}
	}
	
