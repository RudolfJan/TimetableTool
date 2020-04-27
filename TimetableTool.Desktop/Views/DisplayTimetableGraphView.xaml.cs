using Caliburn.Micro;
using DataAccess.Library.Models;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using TimetableTool.Desktop.Models;
using Brushes = System.Windows.Media.Brushes;

namespace TimetableTool.Desktop.Views
	{
	/// <summary>
	/// Interaction logic for DisplayTimetableGraph.xaml
	/// </summary>
	public partial class DisplayTimetableGraphView : UserControl

		{	
		#region DependencyProperties
		public BindableCollection<LocationModel> LocationList
			{
			get { return (BindableCollection<LocationModel>) GetValue(LocationListProperty); }
			set { SetValue(LocationListProperty, value); }
			}

		public static readonly DependencyProperty LocationListProperty =
			DependencyProperty.Register("LocationList", typeof(BindableCollection<LocationModel>), typeof(DisplayTimetableGraphView),
				new FrameworkPropertyMetadata(default(BindableCollection<LocationModel>), FrameworkPropertyMetadataOptions.AffectsRender));

		public int Dummy
			{
			get { return (int) GetValue(DummyProperty); }
			set { SetValue(DummyProperty, value); }
			}

		public static readonly DependencyProperty DummyProperty =
			DependencyProperty.Register("Dummy", typeof(int), typeof(DisplayTimetableGraphView), new FrameworkPropertyMetadata(default(int), 
				FrameworkPropertyMetadataOptions.AffectsRender, OnDummy));

		public int TimeGraphUIChanged
			{
			get { return (int) GetValue(TimeGraphUIChangedProperty); }
			set { SetValue(TimeGraphUIChangedProperty, value); }
			}

		public static readonly DependencyProperty TimeGraphUIChangedProperty =
			DependencyProperty.Register("TimeGraphUIChanged", typeof(int), typeof(DisplayTimetableGraphView), 
				new FrameworkPropertyMetadata(default(int), 
				FrameworkPropertyMetadataOptions.AffectsRender, OnTimeGraphUIChanged));

		public BindableCollection<TimeGraphUIModel> TimeGraphUI
			{
			get { return (BindableCollection<TimeGraphUIModel>) GetValue(TimeGraphUIProperty); }
			set { SetValue(TimeGraphUIProperty, value); }
			}

		public static readonly DependencyProperty TimeGraphUIProperty =
			DependencyProperty.Register("TimeGraphUI", typeof(BindableCollection<TimeGraphUIModel>), typeof(DisplayTimetableGraphView)
				,new FrameworkPropertyMetadata(default(BindableCollection<TimeGraphUIModel>),FrameworkPropertyMetadataOptions.AffectsRender));

		#endregion

	  #region Initialization
		public DisplayTimetableGraphView()
			{
			InitializeComponent();
			var LocationListBinding = new Binding
				{
				Mode = BindingMode.TwoWay,
				Path = new PropertyPath("LocationList"),
				NotifyOnSourceUpdated = true
				};
			BindingOperations.SetBinding(this, LocationListProperty, LocationListBinding);

			var DummyBinding = new Binding
				{
				Mode = BindingMode.TwoWay,
				Path = new PropertyPath("Dummy"),
				NotifyOnSourceUpdated = true
				};
			BindingOperations.SetBinding(this, DummyProperty, DummyBinding);

			var TimeGraphUIChangedBinding = new Binding
				{
				Mode = BindingMode.TwoWay,
				Path = new PropertyPath("TimeGraphUIChanged"),
				NotifyOnSourceUpdated = true
				};
			BindingOperations.SetBinding(this, TimeGraphUIChangedProperty, TimeGraphUIChangedBinding);

			var TimeGraphUIBinding = new Binding
				{
				Mode = BindingMode.TwoWay,
				Path = new PropertyPath("TimeGraphUI"),
				NotifyOnSourceUpdated = true
				};
			BindingOperations.SetBinding(this, TimeGraphUIProperty, TimeGraphUIBinding);

			CanvasSetup();
			// CompositionTarget.Rendering += OnDummy; // Useful?

			EventManager.RegisterClassHandler(typeof(Button), System.Windows.Controls.Primitives.ButtonBase.ClickEvent, new RoutedEventHandler(Button_Click));
			}
		private void CanvasSetup()
			{
			GraphBorder.Width = GraphCanvasSettings.BorderWidth;
			GraphBorder.Height = GraphCanvasSettings.BorderHeight;

			AxisCanvas.Background = GraphCanvasSettings.CanvasBackground;
			AxisCanvas.Width = GraphCanvasSettings.CanvasWidth;
			AxisCanvas.Height = GraphCanvasSettings.CanvasHeight;

			GraphCanvas.Background = GraphCanvasSettings.GraphBackground;
			GraphCanvas.Width = GraphCanvasSettings.GraphCanvasWidth;
			GraphCanvas.Height = GraphCanvasSettings.GraphCanvasHeight;
			Canvas.SetTop(GraphCanvas,GraphCanvasSettings.HorizontalAxisWidth);
			Canvas.SetLeft(GraphCanvas,GraphCanvasSettings.VerticalAxisWidth);
			}

		#endregion

		#region events
		private static void OnDummy(DependencyObject d, DependencyPropertyChangedEventArgs e)
			{
			}

		private static void OnTimeGraphUIChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
			{
			}

		//NotifyCollectionChangedEventHandler OnLocationListCollectionChanged =
		//	delegate(boolobject sender, NotifyCollectionChangedEventArgs args)
		//		{

		//		};

		// This event will handle all buttons
		private void Button_Click(object sender, RoutedEventArgs e)
			{
			// e.OriginalSource
			}


		#endregion

		#region rendering
		protected override void OnRender(DrawingContext drawingContext)
			{
			UpdateLayout();
			if (LocationList != null)
				{
				DrawGrid(GraphCanvasSettings.StartTime / 60, GraphCanvasSettings.VerticalGrids);
				if (TimeGraphUI != null)
					{
					DrawServiceLines(GraphCanvasSettings.StartTime / 60, GraphCanvasSettings.VerticalGrids);
					}
				}
			}

		#endregion
		#region Grid

		private List<Line> GridLines { get; set; } = new List<Line>(); // holds the collection of lines
		private List<UIElement> LocationUiElements { get; set; } = new List<UIElement>(); // holds the collection of text elements


		public void DrawGrid(int firstHour, int hours)
			{
			foreach (var line in GridLines)
				{
				GraphCanvas.Children.Remove(line); // TODO Try block?
				}
			GridLines.Clear();
			foreach (var location in LocationUiElements)
				{
				GraphCanvas.Children.Remove(location); // TODO try block?
				}
			LocationUiElements.Clear();

			double margin = 0; // TODO remove this?
			double width = GraphCanvas.Width - 2 * margin;
			double height = GraphCanvas.Height - 2 * margin;

			// vertical hours per 15 minutes, so divide by 96
			// alt is per hour a grid line
			double gridHeight = height / hours;
			double gridWidth = width / LocationList.Count;

			// Horizontal
			for (int i = 0; i < hours+1; i++)
				{
				var line = GetLine(margin);
				line.X1 = 0;
				line.X2 = width;
				line.Y1 = i * gridHeight;
				line.Y2 = line.Y1;
				var hour = $"{i+firstHour:D2}:00";
				GridLines.Add(line);
				UIElement item= Text(line.X1 - 28, line.Y1 - 5, hour, GraphCanvasSettings.TextColor);
				LocationUiElements.Add(item);
				GraphCanvas.Children.Add(item);
				GraphCanvas.Children.Add(line);
				}

			int j = 0;
			foreach (var item in LocationList)
				{
				var line = GetLine(margin);
				line.X1 = j * gridWidth;
				line.X2 = line.X1;
				line.Y1 = 0;
				line.Y2 = height;
				GridLines.Add(line);
				UIElement locationUI = Text(line.X1, line.Y1 - 15, item, GraphCanvasSettings.TextColor);
				GraphCanvas.Children.Add(locationUI);
				GraphCanvas.Children.Add(line);
				j++;
				}
			}

		private Line GetLine(double margin = 0)
			{
			Line line = new Line();
			line.Margin = new Thickness(margin); ;
			line.Visibility = Visibility.Visible;
			line.StrokeThickness = 0.3;
			line.Stroke = GraphCanvasSettings.GridLineColor;
			return line;
			}

		private TextBlock Text(double x, double y, string text, SolidColorBrush color)
			{
			TextBlock textBlock = new TextBlock();
			textBlock.Text = text;
			textBlock.FontSize = 10;
			textBlock.Foreground = color;
			Canvas.SetLeft(textBlock, x);
			Canvas.SetTop(textBlock, y);
			return textBlock;
			}


		private TextBlock Text(double x, double y, LocationModel location, SolidColorBrush color)
			{
			TextBlock textBlock = new TextBlock();
			textBlock.Text = location.LocationAbbreviation;
			textBlock.ToolTip = location.LocationName;
			textBlock.FontSize = 10;
			textBlock.Foreground = color;
			Canvas.SetLeft(textBlock, x);
			Canvas.SetTop(textBlock, y);
			return textBlock;
			}

		#endregion

		#region DrawData
		private List<Polyline> serviceLines = new List<Polyline>();
		private List<Button> buttonList= new List<Button>();

		public void DrawServiceLines(int firstHour, int hours)
			{
			double margin = 0; // TODO remove this?
			double width = GraphCanvas.Width - 2 * margin;
			double gridWidth = width / LocationList.Count;
			double height = GraphCanvas.Height - 2 * margin;

			// vertical hours per 15 minutes, so divide by 96
			// alt is per hour a grid line
			double gridHeight = height / hours;
			foreach (var item in serviceLines)
				{
				GraphCanvas.Children.Remove(item);
				}
			serviceLines.Clear();

			foreach (var item in buttonList)
				{
				GraphCanvas.Children.Remove(item);
				}
			buttonList.Clear();


			foreach (var instance in TimeGraphUI)
				{
				var line = new Polyline();
				line.Margin = new Thickness(margin);
				;
				line.Visibility = Visibility.Visible;
				line.StrokeThickness = 0.8;
				line.Stroke = Brushes.Black;

				var buttonPoint = instance.DataLine[0];
				System.Windows.Point buttonLocation = new System.Windows.Point
					{
					X = buttonPoint.X * gridWidth,
					Y = (buttonPoint.Y-firstHour*60)*height/(hours*60)
					}; // this is a bit tricky. Point is defined at more places, and we need double precision here to be consistent

				var button = GetButton(instance.ServiceInstanceAbbreviation, buttonLocation.X,
					buttonLocation.Y);
				buttonList.Add(button);
				GraphCanvas.Children.Add(button);

				foreach (var point in instance.DataLine)
					{
					System.Windows.Point p = new System.Windows.Point
						{
						X = point.X * gridWidth,
						Y = (point.Y-firstHour*60)*height/(hours*60)
						}; // this is a bit tricky. Point is defined at more places, and we need double precision here to be consistent
					line.Points.Add(p);
					}

				serviceLines.Add(line);
				GraphCanvas.Children.Add(line);
				}
			}

		private Button GetButton(string instance, double X, double Y)
			{
			var output = new Button();
			output.Foreground = Brushes.Black;
			output.Background = Brushes.Crimson;
			output.Height = 10;
			output.Width = 10;
			output.ToolTip = instance;
			output.Name= Normalize(instance);
			Canvas.SetLeft(output, X-5);
			Canvas.SetTop(output, Y-5);
			return output;
			}

// https://stackoverflow.com/questions/7411438/remove-characters-from-c-sharp-string/7411488
		private string Normalize(string text)
			{
			return string.Join("",
				from ch in text
				where char.IsLetterOrDigit(ch)// || char.IsWhiteSpace(ch)
				select ch);
			}
		#endregion

		}
	}
