using System.Windows.Media;

namespace TimetableTool.Desktop.Views
	{
	public class GraphCanvasSettings
		{
		public static double BorderWidth { get;} = 975;
		public static double BorderHeight { get; } = 675;
		public static double BorderPadding { get; } = 25;
		public static double VerticalAxisWidth { get; } = 90;
		public static double HorizontalAxisWidth { get; } = 90;
		public static double BottomMargin { get; } = 10;
		public static double RightMargin { get; } = 10;

		public static double CanvasWidth
			{
			get
				{
				return BorderWidth - BorderPadding;
				}
			}
		public static double CanvasHeight
			{
			get
				{
				return BorderHeight - BorderPadding;
				}
			}

		public static double GraphCanvasWidth
			{
			get
				{
				return BorderWidth - BorderPadding - HorizontalAxisWidth -RightMargin;
				}
			}
		public static double GraphCanvasHeight
			{
			get
				{
				return BorderHeight - BorderPadding - VerticalAxisWidth - BottomMargin;
				}
			}

		public static int StartTime { get; set; } = 600;
		public static int EndTime { get; set; } = 1440; // 24 hours

		public static int VerticalGrids
			{
			get
				{
				return (EndTime - StartTime) / 60; // assume always 1 hour as primary division
				}
			}

		public static SolidColorBrush BorderColor { get; set; } = Brushes.Maroon;
		public static SolidColorBrush CanvasBackground { get; set;} = Brushes.Khaki;
		public static SolidColorBrush GraphBackground { get; set;} = Brushes.LemonChiffon;
		public static SolidColorBrush GridLineColor { get; set; } = Brushes.Maroon;
		public static SolidColorBrush TextColor { get; set; } = Brushes.Maroon;


		}
	}
