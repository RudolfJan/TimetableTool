using DataAccess.Library.Logic;
using DataAccess.Library.Models;
using ScottPlot;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TimetableTool.Desktop.Models;
using TimetableTool.Desktop.ViewModels;
using Color = System.Drawing.Color;

namespace TimetableTool.Desktop.Views
	{
	/// <summary>
	/// Interaction logic for ScottPlotGraphView.xaml
	/// </summary>
	public partial class ScottPlotGraphForm
		{
		public TimeGraphUIModel SelectedTimeGraph { get; set; }
		public ScottPlotGraph GraphModel { get; set; }
	
		private readonly List<ServiceClassModel> _serviceClassList;

		public ScottPlotGraphForm(ScottPlotGraph graphModel)
			{
			InitializeComponent();
			GraphModel = graphModel;
			_serviceClassList = ServiceClassDataAccess.GetAllServiceClasses();
			DataContext = GraphModel;
			double[] yPositions = new double[25];
			string[] yLabels = new string[25];
			for (int p = 0; p < 25; p++)
				{
				yPositions[p] = -60 * p;
				yLabels[p] = p.ToString("D2") + ":00";
				}

			int i = 0;
			foreach (var graph in GraphModel.TimeGraphUI)
				{
				graph.Plot=PlotService(graph,GraphModel.TimeGraphUI[i].ServiceAbbreviation);
				i++;
				}

			string[] labels = new string[graphModel.LocationList.Count];
			int k = 0;
			foreach (var label in graphModel.LocationList)
				{
				labels[k] = label.LocationAbbreviation;
				k++;
				}

			TimetableGraph.plt.XTicks(labels);
			TimetableGraph.plt.Ticks(invertSignY: true);
			TimetableGraph.plt.YTicks(yPositions,yLabels);
			TimetableGraph.plt.Style(figBg: GetResourceColor("WindowBackground"));
			TimetableGraph.plt.Style(dataBg: GetResourceColor("ControlBackground"));
			TimetableGraph.plt.Grid(enable: true, color: GetResourceColor("GridLine"));
			TimetableGraph.Render();
			}

		public PlottableScatter PlotService(TimeGraphUIModel graph, string serviceAbbreviation)
			{
			double[] dataX = new double[graph.DataLine.Count];
			double[] dataY = new double[graph.DataLine.Count];

			int j = 0;
			foreach (var dataSet in graph.DataLine)
				{
				dataX[j] = dataSet.X;
				dataY[j] = -dataSet.Y;
				j++;
				}

			double xOffset = 0;
			double yOffset = 5;
			TimetableGraph.plt.PlotText(serviceAbbreviation,
				dataX[0] + xOffset, dataY[0] + yOffset, fontName: "Arial", fontSize: 10,
				color: GetResourceColor("GraphText"), bold: true);

			return TimetableGraph.plt.PlotScatter(dataX, dataY, GetLineColor(graph.ServiceType));
			}

		public System.Drawing.Color GetLineColor(string serviceType)
			{
			var serviceClass = ServiceClassDataAccess.GetServiceClassModelFromString(serviceType, _serviceClassList);
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

		private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
			{
			if (TimeGraphDataGrid.SelectedItem != null)
				{
				TimeGraphUIModel model = (TimeGraphUIModel) TimeGraphDataGrid.SelectedItem;
				if (SelectedTimeGraph != null)
					{
					SelectedTimeGraph.Plot.lineWidth = 1;
					}
				model.Plot.lineWidth = 3;
				SelectedTimeGraph = model;
				TimetableGraph.Render();
				}
			}

		private void WindowLoaded(object sender, RoutedEventArgs e)
			{
			MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight*0.9;
			MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;

			}
		}
	}
	