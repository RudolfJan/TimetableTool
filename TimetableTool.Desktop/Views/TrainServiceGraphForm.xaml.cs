using ScottPlot;
using System.Windows;
using TimetableTool.Desktop.Models;
using TimetableTool.Desktop.ReportsCommon;
using TimetableTool.Desktop.ViewModels;
using Color = System.Drawing.Color;

namespace TimetableTool.Desktop.Views
	{
	/// <summary>
	/// Interaction logic for TrainServiceGraphForm.xaml
	/// </summary>
	public partial class TrainServiceGraphForm
		{
		public TrainServiceGraph GraphModel { get; set; }

		private ScottPlot.PlottableScatterHighlight scatterHighLight;
		private TrainPlanningUIModel[] highlightedTrainPlanning;
		private double[] scatterDataX;
		private double[] scatterDataY;
		private int scatterIndex = 0;
		private PlottableScatter marker;
		private double xAxisLabelRotation = 20;

		public TrainServiceGraphForm(TrainServiceGraph graphModel)
			{
			InitializeComponent();
			DataContext = graphModel;
			GraphModel = graphModel;

			}

		public void PlotGraph()
			{

			scatterIndex = 0;

			foreach (var graph in GraphModel.TrainPlanning)
				{
				// If a train does not contain data, do not try to plot
				if (graph.ServicesInTrain.Count > 0)
					{
					graph.Plot = PlotService(graph, GraphModel.Zoom);
					}
				}

			ScottPlotReport.PlotTimeAxis(TimetableGraph.plt, GraphModel.Zoom, GraphModel.Pan);
			ScottPlotReport.PlotLocationAxis(TimetableGraph.plt, GraphModel.LocationList,
				xAxisLabelRotation);

			if (marker != null)
				{
				marker = ScottPlotReport.DrawMarker(TimetableGraph.plt, marker, marker.xs[0], marker.ys[0]);
				}

			if (GraphModel.SelectedTrain != null)
				{
				GraphModel.SelectedTrain.Plot.lineWidth = 3;
				}

			scatterHighLight = TimetableGraph.plt.PlotScatterHighlight(scatterDataX, scatterDataY, lineWidth: 0, markerSize: 0.5);
  		ScottPlotReport.ConfigurePlot(TimetableGraph);

			}

		private void CreateSearchablePointsList()
			{
			int pointCount = 0;
			foreach (var item in GraphModel.TrainPlanning)
				{
				pointCount += item.DataLine.Count;
				}

			scatterDataX = new double[pointCount + 1];
			scatterDataY = new double[pointCount + 1];
			highlightedTrainPlanning = new TrainPlanningUIModel[pointCount + 1]; // keep reverse index to Train

			int i = 0;
			foreach (var item in GraphModel.TrainPlanning)
				{
				foreach (var dataPoint in item.DataLine)
					{
					scatterDataX[i] = dataPoint.X;
					scatterDataY[i] = -dataPoint.Y;
					highlightedTrainPlanning[i] = item;
					i++;
					}

				}
			}

		public PlottableScatter PlotService(TrainPlanningUIModel graphModel, double zoom)
			{
			return TimetableGraph.plt.PlotScatter(graphModel.LocationValue, graphModel.TimeValue, graphModel.LineColor[0]);
      }


		private void Redraw_OnClick(object sender, RoutedEventArgs e)
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
			TrainPlanningUIModel graph = highlightedTrainPlanning[index];
			if (graph != null)
				{
				marker = ScottPlotReport.DrawMarker(TimetableGraph.plt, marker, x, y);
				}
			ChangeHighlightedService(graph);
			TimetableGraph.Render();
			}

	
		private void ChangeHighlightedService(TrainPlanningUIModel graph)
			{
			if (graph != null)
				{
				if (GraphModel.SelectedTrain != null && GraphModel.SelectedTrain.Plot != null)
					{
					GraphModel.SelectedTrain.Plot.lineWidth = 1;
					}
				graph.Plot.lineWidth = 3;
				GraphModel.SelectedTrain = graph;
				}
			}

		private void OnZoomOrScrollChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
			{
			RedrawPlot();
			}

		private void WindowLoaded(object sender, RoutedEventArgs e)
			{
			MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight * 0.9;
			MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
			CreateSearchablePointsList();
			PlotGraph();
			}

		private void Exit_Click(object sender, RoutedEventArgs e)
			{
			Close();
			}

		private void Export_Click(object sender, RoutedEventArgs e)
			{
			// TODO create a better filename?
			TimetableGraph.plt.SaveFig($"{Settings.DataPath}Graph{GraphModel.Route.Id}-{GraphModel.Route.RouteAbbreviation}.png");
			}

		}
	}
