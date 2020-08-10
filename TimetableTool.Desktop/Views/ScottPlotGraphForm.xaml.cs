using DataAccess.Library.Logic;
using ScottPlot;
using System.Windows;
using System.Windows.Media;
using TimetableTool.DataAccessLibrary.Logic;
using TimetableTool.Desktop.Models;
using TimetableTool.Desktop.ReportsCommon;
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
		private double xAxisLabelRotation = 20;

		public ScottPlotGraphForm(ScottPlotGraph graphModel)
			{
			InitializeComponent();
			GraphModel = graphModel;
			DataContext = GraphModel;
			}

		public void PlotGraph()
			{
			scatterIndex = 0;
			int i = 0;
			foreach (var graph in GraphModel.TimeGraphUI)
				{
				graph.Plot = PlotService(GraphModel, graph, GraphModel.TimeGraphUI[i].ServiceAbbreviation, GraphModel.Zoom);
				i++;
				}

			ScottPlotReport.PlotTimeAxis(TimetableGraph.plt, GraphModel.Zoom, GraphModel.Pan);
			ScottPlotReport.PlotLocationAxis(TimetableGraph.plt, GraphModel.LocationList,
				xAxisLabelRotation);

			if (marker != null)
				{
				marker = ScottPlotReport.DrawMarker(TimetableGraph.plt, marker, marker.xs[0], marker.ys[0]);
				}

			if (GraphModel.SelectedTimeGraph != null)
				{
				GraphModel.SelectedTimeGraph.Plot.lineWidth = 3;
				}

			scatterHighLight = TimetableGraph.plt.PlotScatterHighlight(scatterDataX, scatterDataY, lineWidth: 0, markerSize: 0.5);

			ScottPlotReport.ConfigurePlot(TimetableGraph);

			}

		private void PrepareSearchablePointsList()
			{
			var locationCount = GraphModel.LocationList.Count;
			var serviceCount = GraphModel.TimeGraphUI.Count;
			var pointCount = locationCount * serviceCount;
			scatterDataX = new double[pointCount + 1];
			scatterDataY = new double[pointCount + 1];
			highlightedTimeGraphUiModels = new TimeGraphUIModel[pointCount + 1];
			}

		public PlottableScatter PlotService(ScottPlotGraph model, TimeGraphUIModel graph, string serviceAbbreviation, double zoom)
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


				double xOffset = 0.01/zoom;
				double yOffset = 9/zoom;
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
					color: ScottPlotReport.GetResourceColor("GraphText"), bold: true, alignment: alignment);
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
				return Color.Magenta;// Ugly default value
				}
			return Color.FromName(serviceClass.Color);
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
			PrepareSearchablePointsList();
			RedrawPlot();
			}

		private void Redraw_OnClick(object sender, RoutedEventArgs e)
			{
			RedrawPlot();
			}

		private void OnZoomOrScrollChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
			{
			RedrawPlot();
			}

		private void RedrawPlot()
			{
			if (GraphModel != null)
				{
				if (TimetableGraph?.plt != null)
					{
					TimetableGraph.plt.Clear();
					PlotGraph();
					TimetableGraph.Render();
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
				}
			TimetableGraph.Render();
			}
		}
	}
	
